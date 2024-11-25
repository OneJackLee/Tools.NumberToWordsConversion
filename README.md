# Tools.NumberToWordsConversion

The number to words conversion tool is a blazor web application that convert a decimal input
into words and display on the web page.

## Prerequisite

- [Install .NET on Windows, Linux, and macOS](https://learn.microsoft.com/en-us/dotnet/core/install/)

## Get started

1. Clone the repository to your device.
    ```zsh
   git clone {the repository link} 
   ```
2. Open your terminal, and switch the directory to the root folder of this repository.
    ```zsh
    cd Tools.NumberToWordsConversion
   ```
3. On your terminal, execute the following command.
    ```zsh
    dotnet run --project=./Tools.NumberToWordsConversion.Web/Tools.NumberToWordsConversion.Web.csproj
    ```
4. Open your browser, and navigate to [this link](http://localhost:5147).

## Introduction to the web app

### Features

The application takes in 2 input: the currency, and the amount. Once press the submit button,
the application will convert the amount from decimal input to words. 
If the chosen currency doesn't have any subunit, for example JPY, the application will round up
the decimal point to 0.

### Limitation of the web app

Currently, it only supports USD, AUD, MYR and SGD as the input. Due to the limitation of C# decimal,
it currently only supports up to Octillion, that is 10 to the power of 27.

## Additional information
