using FluentValidation;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Response;
using MediatR;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Text;

namespace JobBoard.Application.Command
{
    public record ResumeLanguage(string Name, string ProficiencyLevel);
    public record ResumeEducation(string School, DateTime StartDate, DateTime? EndDate, string Subject, string Level);
    public record ResumeWorkExperience(string Position, string Company, string City, DateTime StartDate, DateTime? EndDate, string Description);
    public record ResumeLink(string Description, string Link);

    public class AddEmployeeResumeCommand : IRequest<ResponseModel>
    {
        public string ResumeName { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string? About { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public IEnumerable<ResumeLanguage> Languages { get; set; }
        public IEnumerable<ResumeEducation> Education { get; set; }
        public IEnumerable<ResumeWorkExperience> WorkExperience { get; set; }
        public IEnumerable<ResumeLink> Links { get; set; }
        public IEnumerable<string> Skills { get; set; }
    }

    public class AddEmployeeResumeHandler : IRequestHandler<AddEmployeeResumeCommand, ResponseModel>
    {
        private readonly IEmployeeResumeRepository _repository;
        private readonly IValidator<AddEmployeeResumeCommand> _validator;
        private readonly IFileService _fileService;

        const string DateFormat = "MM.yyyy";

        public AddEmployeeResumeHandler(IEmployeeResumeRepository repository, IValidator<AddEmployeeResumeCommand> validator,
            IFileService fileService)
        {
            _repository = repository;
            _validator = validator;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(AddEmployeeResumeCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var document = new Document();

            var style = document.Styles[StyleNames.Normal]!;
            style.Font.Name = "Arial";

            var section = document.AddSection();

            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.PageFormat = PageFormat.A4;

            double sectionWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

            var header = section.Headers.Primary;
            var nameParagraph = header.AddParagraph();
            nameParagraph.Format.Font.Size = 20;
            nameParagraph.AddFormattedText(request.Name, TextFormat.Bold);
            AddBorderTop(nameParagraph);

            var contactInfoTab = header.AddTable();

            AddColumns(contactInfoTab, sectionWidth, 3);

            AddRow(contactInfoTab, "E-mail", request.Email);
            AddRow(contactInfoTab, "Phone", request.PhoneNumber);
            AddRow(contactInfoTab, "City", request.City);

            if (request.About is not null)
            {
                var aboutParagraph = header.AddParagraph();
                aboutParagraph.Format.Font.Size = 16;
                aboutParagraph.Format.Alignment = ParagraphAlignment.Center;
                aboutParagraph.AddFormattedText(request.About, TextFormat.Italic);
                AddBorderTop(aboutParagraph);
            }

            if (request.WorkExperience.Any())
            {
                AddTitle(header, "Work Experience");

                foreach (var exp in request.WorkExperience)
                {
                    var experienceTable = header.AddTable();
                    AddColumns(experienceTable, sectionWidth, 2);

                    var firstRow = experienceTable.AddRow();
                    var datesCell = new Cell();
                    datesCell.AddParagraph($"{exp.StartDate.ToString(DateFormat)} - {exp.EndDate?.ToString(DateFormat) ?? "now"}");

                    var positionCell = new Cell();
                    var positionText = positionCell.AddParagraph();
                    positionText.AddFormattedText($"{exp.Position}", TextFormat.Bold);

                    firstRow.Cells.Add(datesCell);
                    firstRow.Cells.Add(positionCell);

                    AddRow(experienceTable, $"{exp.Company} | {exp.City}");

                    AddRow(experienceTable, exp.Description);
                }
            }

            if (request.Education.Any())
            {
                AddTitle(header, "Education");

                foreach (var exp in request.Education)
                {
                    var educationTable = header.AddTable();
                    AddColumns(educationTable, sectionWidth, 2);

                    var firstRow = educationTable.AddRow();
                    var dates = new Cell();
                    dates.AddParagraph($"{exp.StartDate.ToString(DateFormat)} - {exp.EndDate?.ToString(DateFormat) ?? "now"}");

                    var schoolCell = new Cell();
                    var schoolText = schoolCell.AddParagraph();
                    schoolText.AddFormattedText($"{exp.School}", TextFormat.Bold);

                    firstRow.Cells.Add(dates);
                    firstRow.Cells.Add(schoolCell);

                    AddRow(educationTable, exp.Level);

                    AddRow(educationTable, exp.Subject);
                }
            }

            if (request.Languages.Any())
            {
                AddTitle(header, "Languages");

                var languageTable = header.AddTable();
                AddColumns(languageTable, sectionWidth, 4);

                foreach (var lang in request.Languages)
                {
                    AddRow(languageTable, lang.Name, lang.ProficiencyLevel);
                }
            }

            if (request.Skills.Any())
            {
                AddTitle(header, "Skills");

                var text = new StringBuilder();
                foreach (var skill in request.Skills)
                {
                    text.Append($" - {skill}");
                }
                header.AddParagraph(text.ToString());
            }

            if (request.Links.Any())
            {
                AddTitle(header, "Links");

                foreach (var link in request.Links)
                {
                    var p = header.AddParagraph();
                    p.AddFormattedText($"{link.Description}: ", TextFormat.Bold);
                    p.AddText(link.Link);
                }
            }

            var renderer = new PdfDocumentRenderer
            {
                Document = document,
                PdfDocument =
                {
                    PageLayout = PdfPageLayout.SinglePage,
                }
            };

            var fileName = await _fileService.SaveEmployeeResumeFileAsync(renderer);

            var resume = new EmployeeResume
            {
                Name = request.ResumeName,
                EmployeeId = request.EmployeeId,
                FileName = fileName,
            };

            await _repository.AddAsync(resume);

            return new ResponseModel();
        }

        private void AddBorderTop(Paragraph paragraph)
        {
            paragraph.Format.Borders.Top.Width = 1;
        }

        private void AddColumns(Table table, double sectionWidth, int numberOfColumns)
        {
            var columnWidth = sectionWidth / numberOfColumns;

            for(int i = 0; i < numberOfColumns; i++)
            {
                table.AddColumn().Width = columnWidth;
            }
        }

        private void AddRow(Table table, string description, string value)
        {
            var row = table.AddRow();

            var descriptionCell = new Cell();
            var descriptionParagraph = descriptionCell.AddParagraph();
            descriptionParagraph.AddFormattedText($"{description}:", TextFormat.Bold);

            var valueCell = new Cell();
            valueCell.AddParagraph(value);

            row.Cells.Add(descriptionCell);
            row.Cells.Add(valueCell);
        }

        private void AddRow(Table table, string value)
        {
            var row = table.AddRow();
            row.Cells.Add(new Cell());

            var valueCell = new Cell();
            valueCell.AddParagraph(value);

            row.Cells.Add(valueCell);
        }

        private void AddTitle(HeaderFooter header, string text)
        {
            var paragraph = header.AddParagraph();
            paragraph.AddFormattedText(text, TextFormat.Bold);
            paragraph.Format.Font.Size = 20;
            AddBorderTop(paragraph);
        }
    }
}
