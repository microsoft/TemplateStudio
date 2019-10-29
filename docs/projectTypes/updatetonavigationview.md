# Update from HamburgerMenu to NavigationView

WinTS is now using Windows UI NavigationView for NavigationPane projects. With NavigationView being in place, Hamburger menu was marked obsolete and will be removed in a future Windows Community Toolkit Update.

The HamburgerMenu and NavigationView share the same concepts and provide the same functionality with one major exception being the NavigationView take advantage of the new fluent design system. In fact, the NavigationView does everything the HamburgerMenu does and even more. Also generated code is getting easier and more straight-forward.

WinTS projects used a class ShellNavigationItems to manage menu items on Hamburger Menu in two collections (primary and secondary items). This class is no longer necessary, as NavigationView provides the NavigationViewItem class to manage items. Settings page is shown/hidden by setting the property IsSettingsVisible.

Switch from HamburgerMenu to NavigationView is not a drop-in replacement, but we prepared the following step-by-step instructions:

- [Framework: CodeBehind, Language: C#](./updatetonavigationview/codebehind-cs.md)
- [Framework: MVVM Light, Language: C#](./updatetonavigationview/mvvmlight-cs.md)
- [Framework: MVVMBasic, Language: C#](./updatetonavigationview/mvvmbasic-cs.md)
- [Framework: Caliburn.Micro, Language: C#](./updatetonavigationview/caliburnmicro-cs.md)
- [Framework: Prism, Language: C#](./updatetonavigationview/prism-cs.md)
- [Framework: CodeBehind, Language: VB](./updatetonavigationview/codebehind-vb.md)
- [Framework: MVVM Light, Language: VB](./updatetonavigationview/mvvmlight-vb.md)
- [Framework: MVVMBasic, Language: VB](./updatetonavigationview/mvvmbasic-vb.md)

If you use actions on your menu, also look at this updated [document](./navigationpane.md#invokecode)

You can find Community Toolkit's info regarding update from Hamburger Menu to Navigation view [here](https://docs.microsoft.com/windows/communitytoolkit/archive/hamburgermenu#navview)
and NavigationView's doc [here](https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/navigationview)
