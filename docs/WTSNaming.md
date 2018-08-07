# Page Naming
When you add a new page in Windows Template Studio wizard Visual Studio generates some files in your project depending on the selected project type and framework.
This is the files generation result for **Blank Page template** named as **Main**:

**CodeBehind framework**
| File name | Status |
|:-------------|:------------|:------------|
| MainPage.xaml | New |
| MainPage.xaml.cs | New |
| Resources.resw | Modified |
| App.xaml | Modified |
| Styles/Page.xaml | Modified |

**MVVMBasic, MVVMLight, CaliburnMicro and Prism frameworks**
| File name | Status |
|:-------------|:------------|
| MainPage.xaml | New |
| MainPage.xaml.cs | New |
| MainPage.xaml.cs | New |
| Resources.resw | Modified |
| App.xaml | Modified |
| Styles/Page.xaml | Modified |

The sufix Page will be added to the page files by WTS wizard. Template name should not have sufix like page or view.