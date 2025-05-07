docker stop cocktail-api
docker remove cocktail-api
docker build -t cocktails .
docker run -d -p 8080:80 --name cocktail-api cocktails
docker logs cocktail-api
pause