using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Moodle
{
	// this component will be moved to the launcher and moved "the non destroy way"
	[RequireComponent(typeof(MoodleUser))]
	public class MoodleConnector : MonoBehaviour
	{
		public UserToken userToken;
		public MoodleUser moodleUser;
		public List<string> supportedMoodleActivities;
		
		
		private TokenRetriever tokenRetriever;
		private INIParser iniParser;
		
		public string username;

		private Uri moodleURL;
		private string serviceName;
		private bool ready = false;
		
		public bool Ready => ready;


		private void Awake()
		{
			iniParser = new INIParser();
			tokenRetriever = new TokenRetriever();
			moodleUser = GetComponent<MoodleUser>();
		}
		
		private void Start()
		{
			LoadMoodleInfo();
			StartCoroutine(EstablishConnection(Application.isEditor));
		}

		private IEnumerator EstablishConnection(bool requestToken)
		{
			userToken = new UserToken();

			if (requestToken)
			{
				username = "arvrtestuser";
				yield return tokenRetriever.RequestToken(username, "sda!W2d2D8ws", moodleURL.ToString(),
					serviceName);
				userToken = tokenRetriever.Token;
			}
			else
			{
				string[] args = System.Environment.GetCommandLineArgs();
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] == "-token")
					{
						userToken.token = args[i + 1];
					}

					if (args[i] == "-username")
					{
						username = args[i + 1];
					}
				}
			}

			yield return LoadUserData();
			yield return LoadEnrolledCourses();
			yield return LoadCourseActivities();

			ready = true;
		}

		private IEnumerator LoadUserData()
		{
			Dictionary<string, string> postdata = new Dictionary<string, string>();
			postdata.Add("wstoken", userToken.token);
			postdata.Add("wsfunction", "core_webservice_get_site_info ");
			postdata.Add("moodlewsrestformat", "json");

			RESTrequester restreq = new RESTrequester();

			yield return restreq.MoodleRESTrequest(moodleURL.ToString(), postdata);

			var data = (JObject)JsonConvert.DeserializeObject(restreq.GetResponse());
	
			moodleUser.id = data["userid"].ToObject<int>();
			moodleUser.username = data["username"].ToString();
			moodleUser.firstName = data["firstname"].ToString();
			moodleUser.lastName = data["lastname"].ToString();
			moodleUser.fullName = data["fullname"].ToString();

			UnityWebRequest request = UnityWebRequestTexture.GetTexture(data["userpictureurl"].ToString());
			yield return request.SendWebRequest();
			if(request.isNetworkError || request.isHttpError) 
				Debug.Log(request.error);
			else
				moodleUser.profileImage = ((DownloadHandlerTexture) request.downloadHandler).texture;

			// todo: figure out an easier way to query the email adress then core_user_get_users_by_field
		}
		
		
		private IEnumerator LoadEnrolledCourses()
		{
			Dictionary<string, string> postdata = new Dictionary<string, string>();
			postdata.Add("wstoken", userToken.token);
			postdata.Add("wsfunction", "core_enrol_get_users_courses");
			postdata.Add("userid", moodleUser.id.ToString());
			postdata.Add("moodlewsrestformat", "json");

			RESTrequester restreq = new RESTrequester();

			yield return restreq.MoodleRESTrequest(moodleURL.ToString(), postdata);

			var data = JsonConvert.DeserializeObject<List<JObject>>(restreq.GetResponse());

			foreach (var course in data)
			{
				Course c = new Course();
			
				c.ID = course["id"].ToObject<int>();
				c.FullName = course["fullname"].ToString();
				c.ShortName = course["shortname"].ToString();
				c.Summary = course["summary"].ToString();
				
				moodleUser.enrolledCourses.Add(c);
			}
		}

		private IEnumerator LoadCourseActivities() // todo: maybe introduce a switch to only load available activities
		{
			// TODO: Useful calls for automatically updating the completion of an activity (maybe if a user completed the VR scenario..?)
			// core_completion_get_activities_completion_status	=> Return the activities completion status for a user in a course.
			// core_completion_update_activity_completion_status_manually => Update completion status for the current user in an activity, only for activities with manual tracking.
			// core_completion_override_activity_completion_status => Update completion status for a user in an activity by overriding it.

			// iterate all enrolled courses
			foreach (var course in moodleUser.enrolledCourses)
			{
				Dictionary<string, string> postdata = new Dictionary<string, string>();
				postdata.Add("wstoken", userToken.token);
				postdata.Add("wsfunction", "core_course_get_contents");
				postdata.Add("courseid", course.ID.ToString());
				postdata.Add("moodlewsrestformat", "json");
				
				RESTrequester restreq = new RESTrequester();

				yield return restreq.MoodleRESTrequest(moodleURL.ToString(), postdata);
				
				var data = JsonConvert.DeserializeObject<List<JObject>>(restreq.GetResponse());
			
				//iterate all topics
				foreach (var topic in data)
				{
					// iterate all modules/activities
					foreach (var module in topic["modules"])
					{
						string modname = module["modname"].ToString();
						bool visible = module["visible"].ToObject<bool>();
						
						if (visible && supportedMoodleActivities.Contains(modname))
						{
							Activity activity = new Activity();
							activity.ID = module["id"].ToObject<int>();
							activity.Instance = module["instance"].ToObject<int>();
							activity.ModName = module["modname"].ToString();
							activity.ModPlural = module["modplural"].ToString();
							activity.Name = module["name"].ToString();
							
							// we support the module and it is visible so save it for the current user for later use
							course.activities.Add(activity);
						}
					}
				}
			}
		}

		/// <summary>
		/// should load the moodlepath + ip from a file or master server
		/// </summary>
		private void LoadMoodleInfo()
		{
			var config = Resources.Load("config") as TextAsset;
			iniParser.Open(config);

			serviceName = iniParser.ReadValue("Moodle", "serviceName", "");
			moodleURL = new Uri(iniParser.ReadValue("Moodle", "url", ""));
			
			iniParser.Close();
		}
	}
}