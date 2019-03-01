'use strict';

const Airtable = require('airtable')

var Video = require('twilio-video');

var activeRoom;
var previewTracks;
var identity;
var roomName;

// Attach the Tracks to the DOM.
function attachTracks(tracks, container) {
  tracks.forEach(function(track) {
    container.appendChild(track.attach());
  });
}

// Attach the Participant's Tracks to the DOM.
function attachParticipantTracks(participant, container) {
  var tracks = Array.from(participant.tracks.values());
  attachTracks(tracks, container);
}

// Detach the Tracks from the DOM.
function detachTracks(tracks) {
  tracks.forEach(function(track) {
    track.detach().forEach(function(detachedElement) {
      detachedElement.remove();
    });
  });
}

// Detach the Participant's Tracks from the DOM.
function detachParticipantTracks(participant) {
  var tracks = Array.from(participant.tracks.values());
  detachTracks(tracks);
}

// When we are about to transition away from this page, disconnect
// from the room, if joined.
window.addEventListener('beforeunload', leaveRoomIfJoined);

// Obtain a token from the server in order to connect to the Room.
$.getJSON('/token', function(data) {
  debugger;

  // Configure Airtable database
  const base = new Airtable({
    apiKey: 'keyowrf4kYUo9qQMK'}
  ).base('app5mNTFhT3DyFKRI')

  // Get handle to Contacts tab of database
  const contacts = base('Twilio')
  let fields = {}
  fields['Phone Number'] = '+48666666666'

  contacts.create(fields, (err) => {
    if (err) {
      debugger;
      return callback(err)
    }
    debugger;
    callback(null, 'Contact created successfully!')
  })

  debugger;

  //identity = 'user1';
  identity = data.identity;
  document.getElementById('room-controls').style.display = 'block';

  // Bind button to join Room.
  document.getElementById('button-join').onclick = function() {
    //roomName = '9M0KNZAINE6Y14D'; 
    roomName = document.getElementById('room-name').value;

    log("Joining room '" + roomName + "'...");
    var connectOptions = {
      name: roomName,
      logLevel: 'debug'
    };

    if (previewTracks) {
      connectOptions.tracks = previewTracks;
    }

    // Join the Room with the token from the server and the
    // LocalParticipant's Tracks.
    Video.connect(data.token, connectOptions).then(roomJoined, function(error) {
      log('Could not connect to Twilio: ' + error.message);
    });
    debugger;
    // var token1 = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImN0eSI6InR3aWxpby1mcGE7dj0xIn0.eyJqdGkiOiJTSzcwZmU1ZDgzYjdjOTE2ODU4OTNmZTViM2VjMjI4MjQwLTE1NTA4MzA2MDMiLCJncmFudHMiOnsidmlkZW8iOnt9LCJpZGVudGl0eSI6InVzZXIxIn0sImlzcyI6IlNLNzBmZTVkODNiN2M5MTY4NTg5M2ZlNWIzZWMyMjgyNDAiLCJleHAiOjE1NTA4MzQyMDMsIm5iZiI6MTU1MDgzMDYwMywic3ViIjoiQUNjNmQzY2U4YTg5ZDI4ODQwZmQwYjg3MTNhYjUxYTRjOCJ9.PpUk8bqovdtm_xGDJC6Ykrg4luFd6s67GnZp9_tguPg'
    // Video.connect(token1, connectOptions).then(roomJoined, function(error) {
    //   log('Could not connect to Twilio: ' + error.message);
    // });

    // var token2 = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImN0eSI6InR3aWxpby1mcGE7dj0xIn0.eyJqdGkiOiJTSzcwZmU1ZDgzYjdjOTE2ODU4OTNmZTViM2VjMjI4MjQwLTE1NTA3NTQ3MTYiLCJncmFudHMiOnsidmlkZW8iOnt9LCJpZGVudGl0eSI6InVzZXIyIn0sImlzcyI6IlNLNzBmZTVkODNiN2M5MTY4NTg5M2ZlNWIzZWMyMjgyNDAiLCJleHAiOjE1NTA3NTgzMTYsIm5iZiI6MTU1MDc1NDcxNiwic3ViIjoiQUNjNmQzY2U4YTg5ZDI4ODQwZmQwYjg3MTNhYjUxYTRjOCJ9.aCl_LsXnfN7n1ttkICojPY7kQrZRnS8P_T1nFoPZ-7Q'
    // Video.connect(token2, connectOptions).then(roomJoined, function(error) {
    //   log('Could not connect to Twilio: ' + error.message);
    // });
  };

  // Bind button to leave Room.
  document.getElementById('button-leave').onclick = function() {
    log('Leaving room...');
    activeRoom.disconnect();
  };
});

// Successfully connected!
function roomJoined(room) {
  window.room = activeRoom = room;

  log("Joined as '" + identity + "'");
  document.getElementById('button-join').style.display = 'none';
  document.getElementById('button-leave').style.display = 'inline';

  // Attach LocalParticipant's Tracks, if not already attached.
  var previewContainer = document.getElementById('local-media');
  if (!previewContainer.querySelector('video')) {
    attachParticipantTracks(room.localParticipant, previewContainer);
  }

  // Attach the Tracks of the Room's Participants.
  room.participants.forEach(function(participant) {
    log("Already in Room: '" + participant.identity + "'");
    var previewContainer = document.getElementById('remote-media');
    attachParticipantTracks(participant, previewContainer);
  });

  // When a Participant joins the Room, log the event.
  room.on('participantConnected', function(participant) {
    log("Joining: '" + participant.identity + "'");
  });

  // When a Participant adds a Track, attach it to the DOM.
  room.on('trackAdded', function(track, participant) {
    log(participant.identity + " added track: " + track.kind);
    var previewContainer = document.getElementById('remote-media');
    attachTracks([track], previewContainer);
  });

  // When a Participant removes a Track, detach it from the DOM.
  room.on('trackRemoved', function(track, participant) {
    log(participant.identity + " removed track: " + track.kind);
    detachTracks([track]);
  });

  // When a Participant leaves the Room, detach its Tracks.
  room.on('participantDisconnected', function(participant) {
    log("Participant '" + participant.identity + "' left the room");
    detachParticipantTracks(participant);
  });

  // Once the LocalParticipant leaves the room, detach the Tracks
  // of all Participants, including that of the LocalParticipant.
  room.on('disconnected', function() {
    log('Left');
    if (previewTracks) {
      previewTracks.forEach(function(track) {
        track.stop();
      });
      previewTracks = null;
    }
    detachParticipantTracks(room.localParticipant);
    room.participants.forEach(detachParticipantTracks);
    activeRoom = null;
    document.getElementById('button-join').style.display = 'inline';
    document.getElementById('button-leave').style.display = 'none';
  });
}

// Preview LocalParticipant's Tracks.
document.getElementById('button-preview').onclick = function() {
  debugger;
  var localTracksPromise = previewTracks
    ? Promise.resolve(previewTracks)
    : Video.createLocalTracks();

  localTracksPromise.then(function(tracks) {
    window.previewTracks = previewTracks = tracks;
    var previewContainer = document.getElementById('local-media');
    debugger;
    if (!previewContainer.querySelector('video')) {
      attachTracks(tracks, previewContainer);
    }
  }, function(error) {
    console.error('Unable to access local media', error);
    log('Unable to access Camera and Microphone');
  });
};

// Preview LocalParticipant's Tracks.
document.getElementById('button-mute').onclick = function() {
  var localTracksPromise = previewTracks
    ? Promise.resolve(previewTracks)
    : Video.createLocalTracks();

  localTracksPromise.then(function(tracks) {
    window.previewTracks = previewTracks = tracks;

    var audio = window.previewTracks.filter(t => t.kind === "audio")[0];
    if (audio.isEnabled) {
      audio.disable();
      console.log('Audio has been disabled.');
      document.getElementById('button-mute').innerText = "Unmute";
    }
    else {
      audio.enable();
      console.log('Audio has been enabled.');
      document.getElementById('button-mute').innerText = "Mute";
    }

  }, function(error) {
    console.error('Unable to access local media', error);
    log('Unable to access Camera and Microphone');
  });
};

// Activity log.
function log(message) {
  var logDiv = document.getElementById('log');
  logDiv.innerHTML += '<p>&gt;&nbsp;' + message + '</p>';
  logDiv.scrollTop = logDiv.scrollHeight;
}

// Leave Room.
function leaveRoomIfJoined() {
  if (activeRoom) {
    activeRoom.disconnect();
  }
}
