from os import system, rename, listdir, path, makedirs
from shutil import move

"""
This class edits file names and their extensions, and moves the 
unchanged files to a seperate folder. 
"""
class ConvertFiles:
    """
    Args:
        prefix (str): The text needed to idetify the files the user wants changed. 
        fileFormat (str): The desired file format to that the images will be converted to.
        fileNameLength (int): The length that the filename will be shortened to.
    """
    def __init__(self, prefix, fileFormat, fileNameLength):
        self.prefix = prefix
        self.fileFormat = fileFormat
        self.fileNameLength = fileNameLength

        # Booleans used to trigger text in console.
        self.identifiedFiles = False
        self.renamedFiles = False

        # Tracks what files will be added to the "old" folder.
        self.oldFilesList = []

        self.newFileName = None

        self.currentFileFormat = None

        self.changeFileNames()
        self.changeFileTypes()
        self.moveFiles()

    def changeFileNames(self):    
        if not len(prefix) <= 1:
            # Create new directory if needed.
            if not path.exists("./old/"):
                makedirs("./old/")

            # Check for files, that contain the `prefix` string, inside of the current folder.
            for fileName in listdir("."):
                if fileName.startswith(self.prefix):
                    if fileName.endswith(".jpeg"):
                        self.currentFileFormat = "jpeg"
                    elif fileName.endswith(".jpg"):
                        self.currentFileFormat = "jpg"
                    elif fileName.endswith(".png"):
                        self.currentFileFormat = "png"
                    elif fileName.endswith(".gif"):
                        self.currentFileFormat = "gif"
                    elif fileName.endswith(".raw"):
                        self.currentFileFormat = "raw"
                    elif fileName.endswith(".gray"):
                        self.currentFileFormat = "gray"
                    elif fileName.endswith(".tiff"):
                        self.currentFileFormat = "tiff"
                    elif fileName.endswith(".bmp"):
                        self.currentFileFormat = "bmp"
                    elif fileName.endswith(".ico"):
                        self.currentFileFormat = "ico"
                    else:
                        raw_input("Your images are in an unsupported format...")

                    if not self.identifiedFiles:
                        print("Identified files to edit...")
                        self.identifiedFiles = True

                    # Edit the length of the file names, and rename them.
                    fileNameLength = len(fileName)
                    self.newFileName = fileName[fileNameLength-7:]
                    self.oldFilesList.append(self.newFileName)
                    rename(fileName, self.newFileName)

                    if not self.renamedFiles:
                        print("Renaming files...")
                        self.renamedFiles = True

    def changeFileTypes(self):
        print("Converting files to the " + self.fileFormat + " image format.")
        # Calling the `magick convert` command from ImageMagick (http://imagemagick.org/script/index.php) to convert the images.
        for fileName in self.oldFilesList:
            self.newFileName = fileName.replace(self.currentFileFormat, self.fileFormat)
            callString = "magick convert " + fileName + " " + self.newFileName
            system(callString)
            


    def moveFiles(self):
        # Move unconverted files to the "old" folder.
        for fileName in self.oldFilesList:
            move("./" + fileName, "./old/" + fileName)

print("Enter the beginning characters of the files you wish to target (caps sensitive); otherwise, enter through.")
prefix = raw_input("> ")
print

print("Enter the file format you wish to convert to.")
fileFormat = raw_input("> ").lower()
print

print("Please enter the desired length of the new filename (please consider the file extension).")
print("If you do not wish to edit the length of the name, simply enter 0.")
fileNameLength = input("> ")
print

convertFiles = ConvertFiles(prefix, fileFormat, fileNameLength)