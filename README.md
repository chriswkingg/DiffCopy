# DiffCopy

## Overview
Welcome to DiffCopy, a tool that makes large folder copy operations easier! 
This software was designed to allow pausable folder copies while keeping track of previously copied files

## Use Case Example
Say your friend has an external hard drive filled with *legally* obtained movies and you want to copy his collection.
Your friend lets you borrow his external drive but only for a couple hours at a time because he likes to watch movies every night.
Using this software the copy process will be as follows:
1. Run the software and start the copy operation
2. Stop the software, pausing the copy operation, you will have access to the movies already copied onto your local drive
3. When your friend lets you borrow his drive again, resume the copy operation

But what if your friend is constantly expanding his movie collection?  
This is where DiffCopy shines. When run, it will automatically detect the new files and add them to the copy queue. 
This will ensure your copy of the collection can be easily updated when your friend adds a few new movies!

## Using the software
This software is currently under development and is in a very basic state. 
While it is *functional* right now, I would not recommend using it on important files.
Is is also missing a significant amount of features that range from "nice to haves" to "pretty essential"

## Limitations
- Currently, file comparisons are only based on file name, so if a file is updated while the operation is paused, it will NOT be re copied.
- FileList generation is probably slow on a large folder structure (currently untested)
- Overhead
- Not a lot of customization available
- Probably a lot of things im forgetting here!