# Customizable.Menu
## This is a .Net 6 project I developed to help in keeping track of the multitude of url's I use on any given day.

<hr />

_If you are cloning this repository, please enter commands as follows:_

```
$ git clone https://github.com/cdcavell/Customizable.Menu.git
```

<hr />

__Database Migrations CLI Instructions__
<br />
_Before you can use the CLI tools on project, you'll need to add the `Microsoft.EntityFrameworkCore.Design` package to it._
<br />
<br />_Install EF Core Tools:_ `dotnet tool install --global dotnet-ef`
<br />_Upgrade EF Core Tools:_ `dotnet tool update --global dotnet-ef`

_To Initialize:_

```
$ dotnet ef migrations add Initial --context ApplicationDbContext --output-dir Migrations
```

_To Update:_

```
$ dotnet ef migrations add Update --context ApplicationDbContext --output-dir Migrations
```
