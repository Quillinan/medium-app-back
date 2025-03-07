# Medium-app-back

API desenvolvida em C# com .NET e Entity Framework.

Pré-requisitos
Antes de iniciar o projeto, certifique-se de ter instalado:

- .NET SDK
- Banco de dados configurado( exemplo: Microsoft SQL Server)

## Instalação:

Clone o repositório e navegue até a pasta do projeto:

```
git clone https://github.com/Quillinan/medium-app-back.git
cd medium-app-back
```

## Restaure as dependências:

```
dotnet restore
```

## Banco de Dados

Entity Framework migrations:

```
dotnet ef migrations add NomeDaMigração // ( exemplo: InicialCreate)
dotnet ef database update
```

## Executando a API

Para iniciar o servidor localmente, execute:

```
dotnet run
```

A API estará disponível em: http://localhost:5050 (ou conforme configurado no launchSettings.json).
