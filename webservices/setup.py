from distutils.core import setup
from distutils.command.install_data import install_data
from distutils.command.install import INSTALL_SCHEMES
import os
import sys

packages = []
data_files = []

def fullsplit(path, result=None):
    """
    Split a pathname into components (the opposite of os.path.join) in a
    platform-neutral way.
    """
    if result is None:
        result = []
    head, tail = os.path.split(path)
    if head == '':
        return [tail] + result
    if head == path:
        return result
    return fullsplit(head, [tail] + result)

# Tell distutils to put the data_files in plat

for dirpath, dirnames, filenames in os.walk('chaperone_ecell_org_misc'):
    # Ignore dirnames that start with '.'
    for i, dirname in enumerate(dirnames):
        if dirname.startswith('.'): del dirnames[i]
    if '__init__.py' in filenames:
        packages.append('.'.join(fullsplit(dirpath)))
    elif filenames:
        data_files.append([dirpath, [os.path.join(dirpath, f) for f in filenames]])

for scheme in INSTALL_SCHEMES.values():
    scheme['data'] = scheme['purelib']

setup(
    name = "sbml2eml",
    version = '0.0.0',
    url = 'http://www.e-cell.org/',
    author = 'E-Cell Project',
    author_email = 'mozo@sfc.keio.ac.jp',
    description = 'Miscellaneous chaperone.e-cell.org services',
    packages = packages,
    cmdclass = {'install_data': install_data},
    data_files = data_files,
)
