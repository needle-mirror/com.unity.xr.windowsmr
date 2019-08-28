# About the XR SDK Tests :

These Tests will include Cross platform and XR plugin specific tests as a sub module of the xr sdk packages 

# Adding the repo as a submodule :
This pacakge will serve as the main point for tests to enter a pacakage and download for each SDK platform

To make this repo a Sub module 
1.) Website documentation to help 
https://gist.github.com/gitaarik/8735255

Adding a submodule
You can add a submodule to a repository like this:

git submodule add git@github.com:url_to/awesome_submodule.git path_to_awesome_submodule

Getting the submodule's code
git submodule init
This will pull all the code from the submodule and place it in the directory that it's configured to.

Keeping your submodules up-to-date
git submodule update

Making it easier for everyone
It is sometimes annoying if you forget to initiate and update your submodules. Fortunately, there are some tricks to make it easier:
git submodule update --init
This will update the submodules, and if they're not initiated yet, will initiate them.

You can also have submodules inside of submodules. In this case you'll want to update/initiate the submodules recursively:

git submodule update --init --recursive
This is a lot to type, so you can make an alias:

git config --global alias.update '!git pull && git submodule update --init --recursive'
Now whenever you execute git update, it will execute a git pull and a git submodule update --init --recursive, thus updating all the code in your project.

# Technical details
## Requirements

## Release Notes

## Known Issues

## Document revision history
|Date|Reason|
|---|---|
|July 10, 2019|Creation of the test submodule package.|
