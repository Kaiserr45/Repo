# Цель этого скрипта - выполнить произвольные предварительные шаги перед сборкой Dockerfile.
# Скрипт вызывается из шага docker build при передаче его пути в качестве аргумента "PredbuildScript"
# Цель конкретно этого скрипта - создать .tar архив для кэширования шага восстановления пакетов
set -e

vsoPrefix="##"
vsoWarning="${vsoPrefix}vso[task.logissue type=warning;]"
vsoError="${vsoPrefix}vso[task.logissue type=error;]"
vsoSection="${vsoPrefix}[section]"

filter=(-name "*.csproj" -o -name "*.sln" -o -name "[nN]u[gG]et.config")
tarname='prebuild.tar'

#################### READ PARAMETERS #####################
while [ "$1" != "" ]; do
    case $1 in
        --filter )
            shift
            filter=($1)
            ;;
        --tarName )
            shift
            tarname=$1
            ;;
        * )
            echo "Unknown argument $1"
            exit 1
            ;;
    esac
    shift
done
###########################################################

#################### INPUT VALIDATION #####################
if [ ! "$tarname" ]; then
    echo "${vsoError}tarname IS NULL!"
    exit 1
fi
###########################################################

tarname="${tarname//\.tar/}.tar"

echo "Filter is ${filter[@]}"


echo "Packing this files in $tarname:\n $(find . \( ${filter[@]} \) -print0)"
# tarball csproj files, sln files, and NuGet.config
find . \( ${filter[@]} \) -print0 \
    | tar -cvf ${tarname} --null -T -

echo "Created ${tarname}"