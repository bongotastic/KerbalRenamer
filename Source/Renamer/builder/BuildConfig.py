'''
   This scrip builds a Renamer config file from a few csv. This can be useful if anyone wants to add cultures, or add 
   names to existing ones without editing the cfg file itself.
   NOTE: 
   1) the excel workbook is the source to generate the CSV *LASTNAME.transposed.csv The data comes from wikipedia (list of surnamaes)
   and some scraped manually from forebears.io 
   2) The firstname dataset comes from https://raw.githubusercontent.com/MatthiasWinkelmann/firstname-database/master/firstnames.csv 
   
   It would be nice to add from cultures not represented in the MatthiasWinkelmann dataset. However, rather than spend lots of time
   trying to get this right, this folder contains the tool for community contribution. 
'''

class KSPculture:
    def __init__(self, given_name):
        self.name = given_name

        self.MFIRSTNAME1 = []
        self.MFIRSTNAME2 = []
        self.MFIRSTNAME3 = []
        self.FFIRSTNAME1 = []
        self.FFIRSTNAME2 = []
        self.FFIRSTNAME3 = []
        self.LASTNAME1 = []
        self.LASTNAME2 = []
        self.LASTNAME3 = []
        self.FLASTNAME1 = []
        self.FLASTNAME2 = []
        self.FLASTNAME3 = []

    def Write(self, filehandle):
        space = "    "

        filehandle.write(space + "Culture\n "+ space + "{\n" + 2 * space + "name = %s\n"%(self.name))

        for item in ["MFIRSTNAME", "FFIRSTNAME", "LASTNAME"]:
            for i in range(1,4):
                fullname = "%s%d"%(item,i)
                self._writeCollection(filehandle, fullname, self.__getattribute__(fullname))
        if (self.FLASTNAME1):
            self._writeCollection(filehandle, "FLASTNAME1", self.FLASTNAME1)
            self._writeCollection(filehandle, "FLASTNAME2", self.FLASTNAME2)
            self._writeCollection(filehandle, "FLASTNAME3", self.FLASTNAME3)

        filehandle.write("%s}\n"%(space))

    def _writeCollection(self, filehandle, collectionName, collection):
        space = "    "
        collection.sort()
        filehandle.write("%s%s\n%s{\n" % (2 * space, collectionName, 2 * space))
        for name in collection:
            filehandle.write("            key = %s\n"%(name))
        filehandle.write("%s}\n" % (2 * space))

def SanitizeCulture(name):
    name = name.replace(" ", "_")
    name = name.replace("/", "_or_")
    name = name.replace(".", "")
    return name

# Press the green button in the gutter to run the script.
if __name__ == '__main__':

    ## First names #########################################
    data = []

    fin = open("firstnames.world.csv")

    for line in fin:
        data.append(line.strip().split(";"))

    cultures = {}
    for nameline in data[1:]:
        for column in range(2, len(nameline)):
            if nameline[column] != "":
                if (int(nameline[column]) <= -5):
                    continue
                culture = SanitizeCulture(data[0][column])

                if not culture in cultures:
                    cultures[culture] = KSPculture(culture)

                if "?" in nameline[1]:
                    nameline[1] = "MF"
                elif "F" in nameline[1]:
                    nameline[1] = "F"
                elif "M" in nameline[1]:
                    nameline[1] = "M"

                nameline[0] = nameline[0].replace("+", " ")

                if nameline[1] in ["M", "MF"]:
                    cultures[culture].MFIRSTNAME1.append(nameline[0])
                if nameline[1] in ["F", "MF"]:
                    cultures[culture].FFIRSTNAME1.append(nameline[0])

    for c in cultures:
        print(c)

    ## Last names #############################################################
    with open("LASTNAME.transposed.csv", mode='r', encoding='utf-8-sig') as fin:
        for line in fin:
            line = line.strip().split(",")
            if line[0] in cultures:
                for name in line[1:]:
                    if name:
                        cultures[line[0]].LASTNAME1.append(name)
                    else:
                        break
            else:
                print("Missing culture: %s"%(line[0]))

    ## FLast names #############################################################
    with open("FLASTNAME.transposed.csv", mode='r', encoding='utf-8-sig') as fin:
        for line in fin:
            line = line.strip().split(",")
            if line[0] in cultures:
                for name in line[1:]:
                    if name:
                        cultures[line[0]].FLASTNAME1.append(name)
                    else:
                        break
            else:
                print("Missing culture: %s" % (line[0]))

    ## Clean up
    del cultures["East_Frisia"]

    ## Report
    for culture in cultures:
        print(culture+":")
        print("    MFIRSTNAME: %d"%(len(cultures[culture].MFIRSTNAME1)))
        print("    FFIRSTNAME: %d" % (len(cultures[culture].FFIRSTNAME1)))
        print("    LASTNAME: %d" % (len(cultures[culture].LASTNAME1)))
        if (len(cultures[culture].FLASTNAME1)):
            print("    FLASTNAME: %d" % (len(cultures[culture].FLASTNAME1)))

    ## Write outfile ##########################################################
    fout = open("KerbalRenamer.cfg", "w")
    fout.write("KERBALRENAMER\n    {\n    cultureDescriptor = Nationality\n\n")

    for culture in cultures:
        cultures[culture].Write(fout)

    # Profiles section
    fout.write("    profile\n    {\n        name = equiprobable\n")
    for culture in cultures:
        fout.write("        %s = 1\n"%(culture))
    fout.write("    }\n")

    fout.write("}\n")
    fout.close()



