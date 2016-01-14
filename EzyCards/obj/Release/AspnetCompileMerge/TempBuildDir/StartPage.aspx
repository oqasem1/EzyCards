<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartPage.aspx.cs" Inherits="WingtipToys.AdminPage" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">

		function vidplay() {
			var video = document.getElementById("Video1");
			var button = document.getElementById("play");
			if (video.paused) {
				video.play();
				button.textContent = "||";
			} else {
				video.pause();
				button.textContent = ">";
			}
		}

		function restart() {
			var video = document.getElementById("Video1");
			video.currentTime = 0;
		}

		function skip(value) {
			var video = document.getElementById("Video1");
			video.currentTime += value;
		}

		function TestAlert() {
			
			alert("Test Work fine");
		}
		function addToPage() {
			
			
				alert("Test Work fine");
				// calling the API ...
				<%--FB.ui(
				  {
				  	method: 'pagetab',
				  	redirect_uri: 'https://apps.facebook.com/ezystoreapp'
				  },
				  function (response) {
				  	if (response != null && response.tabs_added != null) {

				  		$.each(response.tabs_added, function (pageid) {
				  			alert(pageid);
				  		});
				  	}
				  }
				);--%>
			
		}
		function SetSessionStorage(payload) {
			var variable = "PageId";
			//var payload = document.getElementById("lbl").value;
			sessionStorage.setItem(variable, JSON.stringify(payload));
			alert(variable);
		}

		function GetSessionStorage(variable) {

			var payload = sessionStorage.getItem(variable);

			if (payload == "undefined") {

				return null;

			}

			else {
				alert(payload);
				document.getElementById(result).innerHTML = payload;
				return payload;

			}

		}
	</script>
</head>

<body>
	
	<%--<video id="Video1">
		//  Replace these with your own video files. 
     <source src="demo.mp4" type="video/mp4" />
		<source src="demo.ogv" type="video/ogg" />
		HTML5 Video is required for this example. 
     <a href="demo.mp4">Download the video</a> file. 
	</video>--%>
<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ezygateStore logo.jpg" Height="275px" Width="863px"/>
	<div id="buttonbar" hidden="hidden">
		<button id="restart" onclick="restart();">[]</button>
		<button id="rew" onclick="skip(-10)">&lt;&lt;</button>
		<button id="play" onclick="vidplay()">&gt;</button>
		<button id="fastFwd" onclick="skip(10)">&gt;&gt;</button>
<button id="testAlert2" onclick="addToPage()">Test Alert</button>
		<%--<a href="https://www.facebook.com/dialog/pagetab?app_id=633881506758022&next=https://localhost:44300/AdminPage">Tab Link</a>--%>
	</div>
	<form id="form1" runat="server">

		<button id="dsf" onclick="GetSessionStorage('PageId')" hidden="hidden">Get Value</button>
		<div>
			<%--<a href="https://www.facebook.com/dialog/pagetab?app_id=633881506758022&next=https://localhost:44300/AdminPage">Tab Link</a>--%>
			<p>
				<%--<asp:Button value='Add Ezy Tab Page' onclick="addToPage()" OnClientClick="addToPage()" class="btn btn-success" runat="server"/>--%>
				<input type="submit" value='Add Ezy Tab Page' onclick="addToPage()" onkeypress="addToPage()" class="btn btn-success" runat="server" />
			</p>


		</div>
	</form>
</body>
</html>
