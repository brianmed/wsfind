# wsfind
Windows Search Command Line Utility

## Usage

Usage: wsfind [OPTIONS]
Version: 2020-06-05T23:04:00

Options:
      --path=VALUE           Add contains filter on ItemPathDisplay
      --freetext=VALUE       Add freetext document search
      --scope=VALUE          Limit search to specific folder and its sub-folders
      --directory=VALUE      Limit search to a specific folder
  -v, --verbose              Enable verbose messages
  -h, --help                 Show this message and exit
      --version              Show version message
      
## Example

### Data
PS C:\code\wsfind> type .\foo.txt
friggle
PS C:\code\wsfind> type .\exampleDirectory\bar.txt
friggle

### Query directory and all sub directories

PS C:\code\wsfind> dotnet run -- --freetext=friggle --scope=C:\code\wsfind
C:\Code\wsfind\foo.txt
C:\Code\wsfind\exampleDirectory\bar.txt

### Query specific directory

PS C:\code\wsfind> dotnet run -- --freetext=friggle --directory=C:\code\wsfind
C:\Code\wsfind\foo.txt

### Query by path name

PS C:\code\wsfind> dotnet run -- --path=foo.txt
C:\Code\wsfind\foo.txt
