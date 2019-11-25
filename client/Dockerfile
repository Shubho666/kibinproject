FROM node:12.13-alpine as build

WORKDIR /client

COPY package*.json ./
RUN npm install 
COPY . .
RUN npm run build 

FROM nginx:1.17-alpine as runtime
WORKDIR /client
COPY --from=build /client/dist/client/ /usr/share/nginx/html
COPY --from=build /client/nginx.conf /etc/nginx/conf.d/default.conf
