export interface ResumeLanguage {
    name: string,
    proficiencyLevel: string
}

export interface ResumeEducation {
    school: string,
    startDate: string,
    endDate?: string,
    subject: string,
    level: string
}

export interface ResumeWorkExperience {
    position: string,
    company: string,
    city: string,
    description: string,
    startDate: string,
    endDate?: string
}

export interface ResumeLink {
    description: string,
    link: string
}

export default interface AddEmployeeResumeRequest {
    employeeId: number,
    resumeName: string,
    name: string,
    about?: string,
    email: string,
    phoneNumber: string,
    city: string,
    languages: ResumeLanguage[],
    education: ResumeEducation[],
    links: ResumeLink[],
    workExperience: ResumeWorkExperience[],
    skills: string[]
}