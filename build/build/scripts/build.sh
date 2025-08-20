#!/bin/bash
      
      echo "$(pwd)"

      set -e
      vsoPrefix="##"
      vsoWarning="${vsoPrefix}vso[task.logissue type=warning;]"
      vsoError="${vsoPrefix}vso[task.logissue type=error;]"
      vsoSection="${vsoPrefix}[section]"

      defaultDockerfilePath=$(pwd)
      defaultPrebuildScript="$(pwd)/tfs/scripts/prebuild.sh"
      defaultPostbuildScript="$(pwd)/tfs/scripts/posbuild.sh"
      defaultCustomArguments="--build-arg Version=1.0.0"
      #################### READ PARAMETERS ######################
      while [ "$1" != "" ]; do
          case $1 in
              -f | --file )
                  shift
                  dockerfilePath=$1
                  ;;
              --prebuild )
                  shift
                  prebuildScript=$1
                  ;;
              --postbuild )
                  shift
                  postbuildScript=$1
                  ;;
              -args | --customArguments )
                  shift
                  customArguments=$1
                  ;;
              * ) 
                  usage
                  exit 1
                  ;;
          esac
          shift
      done
      ###########################################################
      
      #################### INPUT VALIDATION #####################
      if [ ! "$dockerfilePath" ]; then
          dockerfilePath="$defaultDockerfilePath"
          echo "${vsoWarning}Dockerfile path is not specified. Trying $dockerfilePath"
      fi

      if [ ! -f $dockerfilePath ]; then
          dockerfilePath="${dockerfilePath}/Dockerfile"

          if [ ! -f $dockerfilePath ]; then
            echo "${vsoError}${dockerfilePath} not found!"
            exit 1
          fi
      fi

      if [ "$prebuildScript" ]; then
        prebuildScriptPath=$(echo "$prebuildScript" | cut -d\" -f2)

        if [ ! -f "$prebuildScriptPath" ]; then
          echo "${vsoError}$prebuildScript not found!"
          exit 1
        fi
      fi

      if [ "$postbuildScript" ]; then
        postbuildScriptPath=$(echo "$postbuildScript" | cut -d\" -f2)

        if [ ! -f "$postbuildScriptPath" ]; then
          echo "${vsoError}$postbuildScript not found!"
          exit 1
        fi
      fi

      if [ ! "$ORG_OPENCONTAINERS_IMAGE_VERSION" ]
      then
        ORG_OPENCONTAINERS_IMAGE_VERSION="default"
      fi

      if [ ! "$BUILD_SOURCESDIRECTORY" ]
      then
        BUILD_SOURCESDIRECTORY="$(pwd)"
      fi
      ###########################################################
      
      ###################### DOCKER BUILD #######################
      DOCKER_BUILDKIT=1

      if [ -f "$prebuildScript" ]; then
        echo "Running: $prebuildScript"
        . $prebuildScript
      fi

      ###########################################################
#      echo "${vsoSection}Preparing test results"
#      prebuildImage="${IMAGENAME}:${ORG_OPENCONTAINERS_IMAGE_VERSION}-prebuild"
#
#      docker build -f ${dockerfilePath} ${customArguments} --target test -t $prebuildImage .
#
#        (docker run --rm -v ${COMMON_TESTRESULTSDIRECTORY}:/TestResults:rw $prebuildImage \
#        && docker image rm --no-prune $prebuildImage) \
#        || (echo "${vsoError}Tests failed" \
#        && docker image rm --no-prune $prebuildImage \
#        && exit 1)

      ###########################################################
      echo "${vsoSection}Building the image"
      command="docker build ${customArguments} \\
        -f ${dockerfilePath} \\
        -t ${IMAGENAME}:${ORG_OPENCONTAINERS_IMAGE_VERSION} \\
        --label \"org.opencontainers.image.created\"=\"${ORG_OPENCONTAINERS_IMAGE_CREATED}\" \\
        --label \"org.opencontainers.image.source\"=\"${ORG_OPENCONTAINERS_IMAGE_SOURCE}\" \\
        --label \"org.opencontainers.image.version\"=\"${ORG_OPENCONTAINERS_IMAGE_VERSION}\" \\
        --label \"org.opencontainers.image.revision\"=\"${BUILD_SOURCEVERSION}\" \\
        --label \"org.opencontainers.image.vendor\"=\"${ORG_OPENCONTAINERS_IMAGE_VENDOR}\" \\
        --label \"ru.ingos.image.businessowners\"=\"${RU_INGOS_IMAGE_BUSINESSOWNERS}\" \\
        \"${BUILD_SOURCESDIRECTORY}\""
      
      ###########################################################
      echo "${vsoSection}Preparing to run:"
      echo "$command"
      
      echo "$command" > ${BUILD_STAGINGDIRECTORY}/"${BUILD_BUILDID}.sh" \
          && ( chmod u+x ${BUILD_STAGINGDIRECTORY}/"${BUILD_BUILDID}.sh" \
          && echo "Starting docker build..." \
          && ${BUILD_STAGINGDIRECTORY}/"${BUILD_BUILDID}.sh" \
          && echo "Docker build finished successfully. Trying to remove the script file..." \
          && rm -f ${BUILD_STAGINGDIRECTORY}/"${BUILD_BUILDID}.sh" \
          && echo "Script file successfully removed." ) \
          || ( rm -f ${BUILD_STAGINGDIRECTORY}/"${BUILD_BUILDID}.sh" \
          && echo "${vsoError}Something went wrong during container creation." \
          && exit 1 )

      if [ -f "$postbuildScript" ]; then
        echo "${vsoSection}Running: $postbuildScript"
        . $postbuildScript
      fi
      ###########################################################