import unittest
import os
from pathlib import Path
import shutil

class TestConverter(unittest.TestCase):

    def test_converter_output(self):
        # output starting test
        print('\nStarten met het converten van test CityGML bestanden naar CityJSON data...')
        os.mkdir('./tests/test_converter_output_test')
        filesConverted = 0


        for fileGml in os.listdir("./tests/" + 'Steden'):
            if fileGml.endswith(".gml"):
                print("\nConversie test van '" + fileGml + "' gaat nu van start.\n")
                os.system(".\converter\citygml-tools to-cityjson ./tests/Steden/" + fileGml)
                fileJson = fileGml[:-3] + "json"
                Path("./tests/Steden/" + fileJson).rename("./tests/test_converter_output_test/" + fileJson)
                print("\nTest Conversie is voltooid.")
                filesConverted += 1
    
        # output ending test
        print('\nAlle bestanden zijn geconvert. Test of conversie goed is verlopen begint nu:')
        # Checking if the files have been converted
        self.assertEqual(filesConverted, len(os.listdir("./tests/Steden")), 'Alle bestanden zijn niet geconvert.')
        # Checking if the files have the correct name
        for fileJson in os.listdir("./tests/test_converter_output_test"):
            self.assertTrue(fileJson.endswith(".json"), 'Alle bestanden zijn niet op de juiste manier geconvert.')


        print('\nTest opschonen...')
        shutil.rmtree('tests/test_converter_output_test')
        print('\nTest compleet.')


if __name__ == '__main__':
    unittest.main()