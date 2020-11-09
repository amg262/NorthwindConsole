# NorthwindConsole

## To install dotnet command line tools

```
dotnet tool install --global dotnet-ef
```

## To scaffold data context and entity classes:

```
dotnet ef dbcontext scaffold "Server=bitsql.wctc.edu;Database=NWConsole_##_XXX;User ID=YYY;Password=ZZZ" Microsoft.EntityFrameworkCore.SqlServer -o Model
```
