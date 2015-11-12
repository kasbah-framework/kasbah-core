HERE=$PWD

# cd ../../src/Kasbah.Core.Admin/wwwroot
# npm install
# NODE_ENV=production npm run compile
# cd ../../../

cd ../../
dnu restore
dnu publish --out docker/80-kasbah-admin/build src/Kasbah.Core.Admin
# remove the unnecessary duplicated wwwroot
# rm -rf build/approot/packages/KasbahWebsite/1.0.0/root/wwwroot
cd $HERE
