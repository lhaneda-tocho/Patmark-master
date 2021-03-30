#!/bin/bash

cd `dirname $0`
dir=`pwd`
echo "<# var ProjectDir = \"$dir\"; #>" > ProjectPath.tt
