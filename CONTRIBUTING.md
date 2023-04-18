# Contribution Guidelines
General guidelines on how to use [Git](#Git), how to format all the [Code](#Code) used in the project and where to put the files in [Unity](#Unity), in order to reduce merge conflicts and spaghetti code :spaghetti:.

Everything we write or name should be in English and easy to understand for everyone. This includes Git Commits, Issues, Pull Requests etc as well as all the Code, comments and file names.

All changes contributed to this repository are under the [MIT License](https://github.com/CodenameAlphamale/FaultWithCuriosity/blob/main/LICENSE).

*For external users, use forks and pull requests into dev instead of using another branch.*

## Git
Using Git is essential and mandatory for this project in order to make sure that people don't mess with the same stuff at the same time and overwrite things for each other.
In order to flow nicely, keep an extra eye on Git at all times and refetch often.

### Branches
The repository is split into the `main` and `dev` branches.

#### Main Branch
This is our production branch, where all the working code is. Updated after a sprint or a bunch of Stories that work together are implemented.

This branch may only be modified with a [Pull Request](https://github.com/CodenameAlphamale/FaultWithCuriosity/pulls) from the `dev` branch.

#### Dev Branch
The branch where our current development is on. This branch is not production ready, feature complete and may contain bugs *(please fix the bugs though :sweat_smile:)*

When bugs are fixed and all the features for the release are implemented, make a pull request from the `dev` branch to the `main` branch.
You may not change the `dev` branch directly, it has to be done with Pull Requests from another branch.

#### Other Branches
If you want to add a new feature or fix something. 

The workflow goes as the following:
- Fetch and make sure that you have the latest updates from the `dev` branch.
- Make a new branch with a describing name, branching off the `dev` branch.
- Work on that branch, pushing your commits to it.
- Test that all new stuff works and that no new bugs have appeared.
- When done, make sure to rebase merge with the new dev
- Create a Pull Request from that branch into the `dev` branch.
- Let someone else review the Pull Request.
- Merge into the `dev` branch with Rebase and Merge.
- Delete the working branch *(NOT the `dev` or `main` branch)*.

### Commits
When implementing something new, make sure to create as small commits as possible. These are called atom commits.
We do it this way in order to be able to just revert small parts of a script instead of having to revert a full script file.

- As soon as a new method that full works are implemented, create a commit for that method only. (If the method needs a few other methods to work, then implement them as well and commit together, otherwise just the single standalone method should be committed.)
- If you change something in the 3D view, create a commit for only that.
- Create a new file or remove a file, create a commit for that change.

#### How to name commits
The commit message/name should be descriptive of what the commit does. Keep the message short, preferably under 50 characters. If needed, explain more in the description.

All commits should be written in the passive tense.

**:white_check_mark: Use**  
`"Add"`, `"Remove"`, `"Fix"`, `"Change"`, `"Update"` etc.

**:x: Do not use**  
`"Added"`, `"Removing"`, `"Fixed"` etc.

A good commit message for adding a new file can be:  
`"Add newfile.cs for player movement"`  
Or changing/updating a method can be:  
`"Change Movement.cs Walk method to check player rotation"`

### Issues
All our Stories are reflected on the [Projects](https://github.com/CodenameAlphamale/FaultWithCuriosity/projects) page and as an [Issue](https://github.com/CodenameAlphamale/FaultWithCuriosity/issues).

When working on a full Story or a Task in a Story, Assign yourself to that issue.

### Pull Requests
When creating a new pull request, make sure to add a short but covering and describing title. In the description go into more detail with all the changes/additions/deletions that the Pull Request does.
You can also add some background to the change if needed/wanted.

#### Linking Issues
If the change has a corresponding Issue, make sure to link that Issue in the Pull Request so that it gets closed when the Pull Request gets merged.

#### Reviews
Assign someone on the team to review your changes. If you got feedback to change things, push those changes to the branch and then request a new review. When all reviews are accepted, then merge the Pull Request.

#### Merging types
When merging into
- `main` branch, use `Squash and Merge`.
- `dev` branch, use `Rebase and Merge`.

This is in order to keep the `dev` branch transparent and easy to revert issues, we want to preserve its whole history.
While the `main` branch should only be merged into when everything is done for a release and therefore, we do not want all the small commits showing up. Instead, we just want one big combined commit per release.


## Code
The most important part in order to keep the codebase clean and easy to understand for everyone.
Always follow the general C# code guildelines if something is not stated otherwise here.

### Formatting
In order to set up formatting in Visual Studio go to Tools -> Options -> Text Editor -> C# -> Code Style -> Formatting

#### Indentation
:ballot_box_with_check: Indent block contents  
:black_large_square: Indent open and close braces  
:black_large_square: Indet case contents  
:black_large_square: Indent case contents (when block)  
:ballot_box_with_check: Indent case labels

Label Indentation: Place goto labels one indent less than current

#### New Lines
New line options for braces: All unchecked :black_large_square:  
New line options for keywords: All unchecked :black_large_square:  
New line options for expressions: All checked :ballot_box_with_check:

#### Spacing
Set spacing for method declarations: All unchecked :black_large_square:  
Set spacing for method calls: All unchecked :black_large_square:  
Set other spacing options: All unchecked :black_large_square:  
Set spacing for brackets: All unchecked :black_large_square:  
Set spacing for delimiters:  
:black_large_square: Insert space after colon for base or interface in type declaration  
:ballot_box_with_check: Insert space after comma  
:black_large_square: Intert space after dot  
:ballot_box_with_check: Insert space after semicolon in "for" statement  
:black_large_square: Insert space before colon for base or interface in type declaration  
:black_large_square: Insert space before comma  
:black_large_square: Intert space before dot  
:black_large_square: Insert space before semicolon in "for" statement

Set spacing for operatiors: Insert space before and after binary operators

#### Wrapping
:ballot_box_with_check: Leave block on single line  
:black_large_square: Leave statement and member declarations on the same line

#### Automatic cleaning
To make sure all this goes smoothly we use automatic Code cleanup :robot:.

Go to Tools -> Options -> Text Editor -> Code Cleanup and enable the option check Run Code Cleanup profile on Save :ballot_box_with_check:

Then click on Configure Code Cleanup and add the following options:
- Format document
- Remove unnecessary Imports or usings
- Sort Imports or usings
- Apply file header preferences
- Remove unnecessary casts
- Apply new() preferences

#### Indentation
Everyone should use tabs for indentation with a tab size of 4.
Also make sure that all your code is correctly indented since the code cleanup might not fix everything.

This is set up in Visual Studio by going to Tools -> Options -> Text Editor -> C# -> Tabs
and set Indenting to Smart, Tab size and Indent size to 4 and Keep tabs.

#### New Lines
At the end of each file, there should be **one** new empty line with nothing on it. Not two, three or more. This is to remove unnecessary Git changes.

Never have two or more new empty lines in the code, it will just make it hard to read the code and a lot to scroll through.
So, make sure to purge away any extra new lines. Though spacing out different parts of the code and the methods with one empty new line is needed.

Always make sure that all your code does not have extra empty lines since the code cleanup might not purge everything automatically.

#### No Spaghetti please
Using Stackoverflow, YouTube and Googling is not weird, but please make sure to not copy and paste a lot of unnecessary spaghetti code :spaghetti:.
Also, make sure to follow the indentation and new line guidelines stated above when copying something.

It is always recommended to write the code manually instead so you get a better understanding and that it will follow our guidelines.

### Naming convention
Everything should always be written in English.

Never use abbreviations, if they are not **super** standard. Writing one more character is not that much harder, espcially when we have auto complete.
This will also improve the readabilty of the code a lot for everyone else and yourself when you return to that code in the future.

Always make sure to describe as much as possible, for example instead of using `rectangle` for an object's hitbox, then use `hitbox`.

If you have multiple variables that are the same thing for a different variation, make sure both of them have a similar name and structure.
For example `redBoxHitbox` and `blueCircleHitbox` instead of `hitboxRedBox` and `circleHitboxBlue`.
Since if you have one varaible and want the other one, then it should be easy to find that one from its name.

#### UPPERCASE and lowercase
Classes, Methods, Interfaces and Properties should use PascalCase.
Where the first letter is UPPERCASE and the first letter of each new word should also be UPPERCASE. The rest should be lowercase. For example: `MoveRedCircle`.

Variables should use camelCase.
Where the first word is lowercase and the first letter of each new word should be UPPERCASE. For example: `redCircleHitbox`.

### Commenting
Always comment your code! You do not have to comment every single line, that is just annoying. Instead try to use an explaning way of coding that are simple to understand without the need of comments.
Sometimes for big clumps of code that may be hard to understand though, then you must comment that part.

#### XML Comments
This is the most important part to comment. XML comments are a custom type of comment used in C# made for variables and methods.
When you hover over a variable or a method name, it will show a description if you have added XML comments to it, even if it's in a another file.
This makes is much faster, so you do not have to guess or find that other file and read the normal comment in that file.

```cs
/// <summary>
/// Parses all valid arguments, removing everything else and combines it into a string.
/// </summary>
/// <param name="arguments">A <see cref="string"/> array with all input arguments.</param>
/// <returns>The parsed arguments as a <see cref="string"/>.</returns>
public string Parse(string[] arguments) { }
```

Unity does not generate these by default, so they need to be added manually. Just write `///` above a method or variable and Visual Studio will auto complete all the needed fields for you to fill in. 


## Unity

### Folder structure
- Assets
  - Materials - Combined Textures into materials
  - Models - Base models
  - Prefabs - Models with hitboxes, rigidboies and such
  - Scenes - Different scenes in the game such as dungeno and menu
  - Scripts - All needed scripts
  - Sound - All sound effects and music
  - Textures - Base textures

### Future proofing
When implementing something new, always think after one extra time if the feature will be used with something else in the future. If so, then make sure to make variables that should be modified easily accessible.
As for health, if multiple different things should heal or take damage to health in the future, then think about that a little bit when implementing the base health scripts.

For keybindings we will not hardcode all the keys but instead use a keybinding system. Which system is not yet decided.
