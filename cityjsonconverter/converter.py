import os
from pathlib import Path

def dirOrFile():
    a = input("Wilt u een aparte bestand of een complete folder converten? (b/f): ")
    wd = os.getcwd()
    while a != "b" and a != "f":
        a = input("\n'" + a + "' is een ongeldige invoerwaarde. Voer een geldige invoerwaarde in. (b/f): ")
    if a == "b":
        # define file name
        fileName = input("\nVoer de naam van het bestand in zonder de .gml extensie: ")
        convertFile(fileName)
    elif a == "f":
        # define directory name
        dirName = input("\nVoer de naam van de folder in: ")
        convertDir(dirName)

def convertFile(fileName):

    # enters a loop to start program and catch errors
    while True:	

        # define file name with .gml and .json extension
        fileGml = fileName + ".gml"
        fileJson = fileName + ".json"

        # checks if .gml file exists
        if os.path.isfile(fileGml) == True:

            # if .gml file exists check if a .json file already exists with the same name 
            if os.path.isfile(fileJson) == True:
                # print("\n" + fileGml + " has been found.\n")

                # if .json file exists ask user what to do
                exist = input("\nEr bestaat al een .json bestand met dezelfde naam. Kies één van de volgende opties:\n\n1. Oveschrijf het bestaande bestand \n2. Ga door met de conversie van het .json bestand met een aangepaste naam\n3. Sluit de applicatie af\n\n(1/2/3): ")

                # if user wants to overwrite existing file
                if exist == "1":
                    print("\nConversie van '" + fileGml + "' en overschrijven naar '" + fileJson + "' gaat nu van start.\n")
                    os.system(".\converter\citygml-tools to-cityjson .\\" + fileGml)
                    print("\Conversie en overschrijven is voltooid.\n")

                # if user want to convert to a .json file with an adjusted name
                elif exist == "2":

                    # ask  the user for the adjusted .json file name
                    adjustedName = input("\nVoer de aangepaste naam voor het .json bestand in: ")
                    print("\n" + "Conversie van '" + fileGml + "' naar '" + adjustedName + ".json' gaat nu van start.\n")
                    
                    # change already existing .json file name temporarily
                    os.rename(fileJson, "temp.json")

                    # convert .gml file to .json file
                    os.system(".\converter\citygml-tools to-cityjson .\\" + fileGml)
                    print("\nConversie is voltooid.")

                    # rename .json file to adjusted name
                    os.rename(fileJson, adjustedName + ".json")

                    # rename already existing .json temporary file name back to original .json file name
                    os.rename("temp.json", fileJson)
                
                # if user wants to exit program
                elif exist == "3":
                    print("De applicatie wordt nu afgesloten.")
                    exit()


            # if .json file doesn't already exists, start conversion
            else:
                print("\n" + fileGml + " is gevonden. Conversie gaat nu van start.\n")
                os.system(".\converter\citygml-tools to-cityjson .\\" + fileGml)
                print("\nConversie is voltooid.")

            # after conversion is complete, break out of loop
            break
    
        # checks if .gml file doesn't exist
        elif os.path.isfile(fileGml) == False:

            # if .gml file doesn't exist, ask user to enter another file name
            fileName = input("\nBestand '" + fileName + "' is niet gevonden, voer een bestaande .gml bestand in: ")

            # program will now go back to the start of the loop to convert the newly entered file name


    # after conversion is complete, ask user if they want to convert another file
    continuee = input("\nWil je nog een bestand converten? (j/n): ")
    b = True

    # enter a loop to start to evaluate reply and catch errors
    while b:
        # if user wants to convert another file, call convert() function
        if continuee == "j":
            # newFileName = input("\nVoer de naam van het bestand in wat je wilt converten: ")
            # print("")
            # convertFile(newFileName)
            dirOrFile()
            b = False

        # if user doesn't want to convert another file, exit program
        elif continuee == "n":
            b = False
            input("\nBedankt voor het gebruiken van de converter. Klik op [enter] om de applicatie af te sluiten.")

        # if user enters an invalid reply, ask user to enter another reply
        elif continuee != "j" and continuee != "n":
            continuee = input("\n'" + continuee + "' is een ongeldige invoerwaarde. Voer een geldige invoerwaarde in. (j/n): ")


def convertDir(dirName):
    
        # checks if directory exists
    if os.path.isdir(dirName) == True:
        print("\n'" + dirName + "' is gevonden.\n")
        os.mkdir(dirName + "_CityJSON")
        for fileGml in os.listdir(".\\" + dirName):
            if fileGml.endswith(".gml"):
                # os.system(".\converter\citygml-tools to-cityjson ..\\" + dirName + "\\" + fileGml)
                print("\nConversie van '" + fileGml + "' gaat nu van start.\n")
                os.system(".\converter\citygml-tools to-cityjson ./" + dirName + "/" + fileGml)
                fileJson = fileGml[:-3] + "json"
                Path("./" + dirName + "/" + fileJson).rename("./" + dirName + "_CityJSON/" + fileJson)
                print("\nConversie is voltooid.")
            
        b = input("\nAlle bestanden in '" + dirName + "' zijn geconvert. Wil je nog een bestand of folder converten? (j/n): ")
        while b != "j" and b != "n":
            b = input("\n'" + b + "' is een ongeldige invoerwaarde. Voer een geldige invoerwaarde in. (j/n): ")
        if b == "j":
            dirOrFile()
        if b == "n":
            input("\nBedankt voor het gebruiken van de converter. Klik op [enter] om de applicatie af te sluiten.")
    else:
        print("\n'" + dirName + "' is niet gevonden.\n")
        dirOrFile()

# call convert() function
dirOrFile()