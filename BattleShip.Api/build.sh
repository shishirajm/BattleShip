dotnet publish -c Release -o out

docker build -t battleship .

docker run -d -p 55201:55201 --name battleship battleship