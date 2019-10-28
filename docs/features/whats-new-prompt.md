# What's New Prompt

This feature will display a customizable prompt when the app is used for the first time after it's been updated.

## Testing this feature

This feature works by storing the current version number of the app being used and displaying the dialog when the version number of the app is different to what it stored previously.  
The version number of the app will change every time you release a new update through the store but you can recreate this by manually changing the version number.

Test this feature by following these steps:

- Run the app. (You'll see no prompt.)
- Close the  app
- In the project open `package.appxmanifest`
- Under **Packaging** change the version number (one or more of the major, minor, or build.)
- Run the app again and you'll see the dialog.
- Run it again without changing the version number and you won't see the dialog.

Be sure to update the contents of the dialog every time you release an update so your users always know about what's new.
