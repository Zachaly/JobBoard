# build stage
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY . .
RUN npm --verbose install
 
EXPOSE 3000
CMD [ "npm", "run", "serve" ]