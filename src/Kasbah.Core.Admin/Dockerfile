FROM microsoft/aspnet

COPY build /app

WORKDIR /app
COPY startup.sh /app/startup.sh
RUN chmod +x startup.sh

EXPOSE 5004

ENTRYPOINT ["sh", "startup.sh"]

