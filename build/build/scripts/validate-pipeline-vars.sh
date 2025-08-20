printenv
if [ ! "${BUILD_SOURCESDIRECTORY}" ]; then
          echo "${vsoError}BUILD_SOURCESDIRECTORY IS NULL! Perhaps you are running the script out of TFS pipeline?"
          exit 1
      fi
      
      if [ ! "${IMAGENAME}" ]; then
          echo "${vsoError}IMAGENAME IS NULL! It MUST be specified in TFS pipeline as variable."
          exit 1
      fi
      
      if [ ! "${ORG_OPENCONTAINERS_IMAGE_CREATED}" ]; then
          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_CREATED IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
          exit 1
      fi
      
#      if [ ! "${ORG_OPENCONTAINERS_IMAGE_AUTHORS}" ]; then
#          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_AUTHORS IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
      if [ ! "${ORG_OPENCONTAINERS_IMAGE_VERSION}" ]; then
          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_VERSION IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
          exit 1
      fi
      
      if [ ! "${ORG_OPENCONTAINERS_IMAGE_VENDOR}" ]; then
          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_VENDOR IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
          exit 1
      fi
      
#      if [ ! "${ORG_OPENCONTAINERS_IMAGE_TITLE}" ]; then
#          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_TITLE IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${ORG_OPENCONTAINERS_IMAGE_DESCRIPTION}" ]; then
#          echo "${vsoError}ORG_OPENCONTAINERS_IMAGE_DESCRIPTION IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${RU_INGOS_IMAGE_SERVICE_GROUP}" ]; then
#          echo "${vsoError}RU_INGOS_IMAGE_SERVICE_GROUP IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${RU_INGOS_IMAGE_SERVICE_TYPE}" ]; then
#          echo "${vsoError}RU_INGOS_IMAGE_SERVICE_TYPE IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${RU_INGOS_IMAGE_API_VERSION}" ]; then
#          echo "${vsoError}RU_INGOS_IMAGE_API_VERSION IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${RU_INGOS_IMAGE_PATH_PREFIX}" ]; then
#          echo "${vsoError}RU_INGOS_IMAGE_PATH_PREFIX IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi
      
#      if [ ! "${RU_INGOS_IMAGE_PATH_BASE}" ]; then
#          echo "${vsoError}RU_INGOS_IMAGE_PATH_BASE IS NULL! Perhaps you did not run buid-init.sh or no value was found in config?"
#          exit 1
#      fi