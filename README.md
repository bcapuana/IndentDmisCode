# IndentDmisCode
Program to fix the indentation of DMIS code in DMI Files.

## Usage
This project generates a single file .net core 3.1 WPF app. Upon launching you will be prompted to select a folder. After clicking go, the indentation will in all dmi files in the selected folder will have the indentation fixed.

## Example
The following code:

```
M(CREATE_OUTPUT_FILE_PATHS)=MACRO
DECL/GLOBAL,CHAR,512,OutputFolder,BaseFileName,ResFileName,PointFileName,DatabaseFileName
OutputFolder = ASSIGN/CONCAT(ProgramDirectory,'RESULTS\')
CALL/EXTERN,DME,'CREATEFOLDER',OutputFolder
BaseFileName = ASSIGN/CONCAT(OutputFolder,DrawingNumber,'_',SerialNumber,'_',SafeFileDate,'_',SafeFileTime)
ResFileName = ASSIGN/CONCAT(BaseFileName,'.RES')
PointFileName = ASSIGN/CONCAT(BaseFileName,'_Points.txt')
ENDMAC
```

will become:

```
M(CREATE_OUTPUT_FILE_PATHS)=MACRO
  DECL/GLOBAL,CHAR,512,OutputFolder,BaseFileName,ResFileName,PointFileName,DatabaseFileName
  OutputFolder = ASSIGN/CONCAT(ProgramDirectory,'RESULTS\')
  CALL/EXTERN,DME,'CREATEFOLDER',OutputFolder
  BaseFileName = ASSIGN/CONCAT(OutputFolder,DrawingNumber,'_',SerialNumber,'_',SafeFileDate,'_',SafeFileTime)
  ResFileName = ASSIGN/CONCAT(BaseFileName,'.RES')
  PointFileName = ASSIGN/CONCAT(BaseFileName,'_Points.txt')
ENDMAC
```