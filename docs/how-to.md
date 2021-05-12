# How to...

##  ... create a new project:

1. Install the extension following the steps from [Installing the Extension](https://github.com/Microsoft/WindowsTemplateStudio/blob/dev/docs/getting-started-extension.md)	
2. Open Visual Studio 2019
3. From the New Project Dialog select "Create New Project" select "Windows Template Studio (Universal Windows)" for UWP projects, "Windows Template Studio (WPF .NET Core)" for WPF projects or "App (WinUI 3 in Desktop)" for WinUI 3 projects creation and click next.
4. Click Create
5. Select a project type

   _Details: By clicking the details link you can see a description of the project type, licenses and dependencies._

6. Select a design pattern:

   _Details: By clicking the details link you can see a description of the design pattern, the author, licenses and dependencies._

7. Add the pages/features/services and testing projects you want to add to your project.

   Pages, Features, Services and Testing options are separated in four categories, but their behaviour is the same:
    * You can add any item by clicking on a template card or by keyboard selection and pressing enter. 
    * **Rename:** After adding a template you can change it's default name in the project details column on the right. Validations are applied (cannot be empty, contain whitespaces, …) In case validation is not successful, a warning message is shown.
		There are some features that do not allow renaming, in this case the textbox is disabled.
    * **Multi Instance vs Single Instance:** There are items that can be added various times to the project and others that can be added only once (like the settings page). The number of pages added is indicated at the bottom of the template card by a counter. In case of Single Instance Templates it states added. 
    * **Dependencies:** Some templates depend on others. When you add such a template, its dependencies are automatically added. You cannot remove a dependency whilst the dependent template is not removed (a warning message is shown)
    * **Details:** By clicking the details link you can see a description of the template, the author, licenses and dependencies.

8. Review your project configuration
	The project details configuration works like a shopping cart. It provides an overview about everything you configured for your project and allows adjustments.
    * **Dropdowns:** The project type/design pattern dropdown allows you to change project type and design pattern. If you change it, your project will be reset, a warning is shown if you already added pages/features 
    * **Delete:** You can delete items using the Del key or the delete button. If there is a dependency on the template it cannot be deleted and warning is shown. The last page cannot be deleted, there must always be at least one page
    * **Reorder:** You can reorder the pages (except settings page) by drag and drop or with Ctrl + Arrow Keys 
    * **Change names:** You can rename items as described above

9) Create: By clicking on create your project is created.

## ... add a new page or feature:

Once your project is created you can add new pages/features.

1. Open the context menu on the main project node (Universal Windows project for UWP, WPF project for WPF projects)
2. Select Windows Template Studio -> New page, New feature, New service or New testing project

3. Select an item. Only one item can be selected at a time.

   _Details: By clicking the details link you can see a description of the template, the author, licenses and dependencies._

4. Click next
5. Review the changes:
   The changes summary view allows you to preview the changes that will be introduced in your project. 

   Files are shown in 4 categories on a list on the right side:
    * Modified files:
      Files that will be modified. Observe the changes in the code viewer in the center of the wizard
    * New Files:
      New files that will be added to your project. Observe the new files code in the code viewer.
    * Unchanged Files:
      Files that were generated for the new page/feature but already where in your project and have not been modified
    * Failed merges:
      To include the new feature/page WTS tried to modify files from your project, but could not do so.
      The center of the screen shows the reason of the error message and the code that was tried to be introduced.

6. Create: Click create to integrate the changes in your project. There is the option to check "Do not merge changes" which will result in a generation in a temporary folder.
