*Recommended Markdown viewer: [Markdown Editor VS Extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2)*

# Steps to include new item generation
Please follow the instructions to include the new item into you project:

You can find all referenced files in the temporary generation folder: {{tempPath}}

## New files:
Copy and add the following files to your project:
* [NewFile.cs](NewFile.cs)

## Modified files:
To integrate your new item with the existing project apply the following changes:

### Changes required in file 'ModifiedFile_Success.cs':
```CSHARP
**MERGECODEPLACEHOLDER**
```

Preview the changes in: [ModifiedFile_Success.cs](ModifiedFile_Success.cs)

### Changes required in file 'ModifiedFile_Error.cs':
```CSHARP
**MERGECODEPLACEHOLDER**
```

Preview the changes in: 
* TestDescription


## Conflicting files:
These files already exist in your project and were also generated as part of the new item.
Please compare and make sure everything is in the right place:
* Temp: [ConflictFile.cs](ConflictFile.cs), ProjectFile: [ConflictFile.cs](about:/{{projectPath}}/ConflictFile.cs)

## Unchanged files:
These files already exist in your project, no action is necessary:
* [UnchangedFile.cs](UnchangedFile.cs)
