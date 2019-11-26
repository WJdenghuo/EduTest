/* eslint-disable require-jsdoc */

// initialize userId/roomId
$('#userId').val('user_' + parseInt(Math.random() * 100000000));
$('#roomId').val('889988');

let rtc = null;

$('#join').on('click', function(e) {
  e.preventDefault();
  console.log('join');
  if (rtc) return;
  const userId = $('#userId').val();
  const roomId = $('#roomId').val();
  const config = genTestUserSig(userId);
  rtc = new RtcClient({
    userId,
    roomId,
    sdkAppId: config.sdkAppId,
    userSig: config.userSig
  });
  rtc.join();
});

$('#publish').on('click', function(e) {
  e.preventDefault();
  console.log('publish');
  if (!rtc) {
    Toast.error('请先加入房间！');
    return;
  }
  rtc.publish();
});

$('#unpublish').on('click', function(e) {
  e.preventDefault();
  console.log('unpublish');
  if (!rtc) {
    Toast.error('请先加入房间！');
    return;
  }
  rtc.unpublish();
});

$('#leave').on('click', function(e) {
  e.preventDefault();
  console.log('leave');
  if (!rtc) {
    Toast.error('请先加入房间！');
    return;
  }
  rtc.leave();
  rtc = null;
});
$('#screen').on('click', function (e) {
    e.preventDefault();
    console.log('screen');
    if (!rtc) {
        Toast.error('请先加入房间！');
        return;
    }
    let videoProfile = rtc.localStream_.videoProfile_;
    console.log(videoProfile.width);
    rtc.localStream_.setVideoProfile('1080p');
    navigator.mediaDevices.getDisplayMedia({ audio: true, video: true}).then(stream => {
        const audioTrack = stream.getAudioTracks()[0];
        const videoTrack = stream.getVideoTracks()[0];

        rtc.localStream_.replaceTrack(videoTrack);
        rtc.localStream_.replaceTrack(audioTrack);
    });
});
$('#settings').on('click', function(e) {
  e.preventDefault();
  $('#settings').toggleClass('btn-raised');
  $('#setting-collapse').collapse();
});
$("#cameraId").on("change", function () {
    let id = $(this).val();
    console.log(id);
    rtc.localStream_.setVideoProfile('480p');
    rtc.localStream_.switchDevice('video', id).then(() => {
        // camera切换成功
    });
})
$("#changeV").on("click", function () {
    
})

