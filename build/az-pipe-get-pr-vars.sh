#!/bin/bash
# Runs as part of Azure Pipeline and sets correct version and docker tag
# respects PR builds

if [ "$(Build.SourceBranchName)" == "master" ]; then
  echo "Versioning for master build"
  version=$(echo "$BUILD_BUILDNUMBER" | cut -d "-" -f 2)
  echo "##vso[task.setvariable variable=version]$version"
  echo "##vso[task.setvariable variable=image_tag]$version"
elif [ -n "$SYSTEM_PULLREQUEST_PULLREQUESTID" ]; then
  echo "Versioning for PR build"
  version=0.$SYSTEM_PULLREQUEST_PULLREQUESTID.$BUILD_BUILDID
  echo "##vso[task.setvariable variable=version]$version"
  echo "##vso[task.setvariable variable=image_tag]$version-pr"
else
  echo "Branch is not for master nor a pull request"
  exit 1
fi