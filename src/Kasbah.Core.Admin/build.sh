cd wwwroot/
npm install
NODE_ENV=production npm run compile
cd ../

dnu restore
dnu publish --framework dnxcore50 --out build --no-source .
# remove the unnecessary duplicated wwwroot
rm -rf build/approot/packages/MyMonthlyTee.Website.Public/1.0.0/root/wwwroot
