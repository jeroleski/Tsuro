FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

ENV PORT 80

#COPY dependacies /app/dependencies

#RUN dependencies argument

COPY . /app/

CMD [ "dotnet", "run" ]