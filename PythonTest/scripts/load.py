#!/usr/bin/env python

"""
This loads all the required .NET modules so you can use them in python. 
"""

import clr
import System

clr.AddReference('WeaponWizard')
clr.AddReference('MonoGame.Framework')
