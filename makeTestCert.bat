makecert -r -pe -n "CN=WGestures Test CA" -ss CA -sr CurrentUser -a sha256 -cy authority -sky signature -sv WGesturesTestCA.pvk WGesturesTestCA.cer

makecert -pe -n "CN=YingDev.com SPC" -a sha256 -cy end -sky signature -ic WGesturesTestCA.cer -iv WGesturesTestCA.pvk  -sv WGesturesTestSPC.pvk WGesturesTestSPC.cer

pvk2pfx -pvk WGesturesTestSPC.pvk -spc WGesturesTestSPC.cer -pfx WGesturesTestSPC.pfx

move WGesturesTestSPC.pfx YingDevSPC.pfx
move WGesturesTestCA.cer WGestures.App\cert\YingdevCA.cer

del WGesturesTestCA.cer WGesturesTestCA.pvk WGesturesTestSPC.cer WGesturesTestSPC.pvk

pause