# Imports the E-Cell_IDE library
import sys
import System
initDir = System.Environment.GetEnvironmentVariable("IRONPYTHONSTARTUP")
sys.path.append(System.IO.Path.GetDirectoryName(initDir))
sys.path.append(System.IO.Path.GetDirectoryName(initDir) + "\\FePy")
from EcellIDE import *

print "# Imports following modules by default."
print "from EcellIDE import *"
print ""

