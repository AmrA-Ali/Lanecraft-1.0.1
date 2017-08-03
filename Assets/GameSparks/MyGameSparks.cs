#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using GameSparks.Core;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_MAP_ADD : GSTypedRequest<LogEventRequest_MAP_ADD, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_MAP_ADD() : base("LogEventRequest"){
			request.AddString("eventKey", "MAP_ADD");
		}
		
		public LogEventRequest_MAP_ADD Set_id( string value )
		{
			request.AddString("id", value);
			return this;
		}
		
		public LogEventRequest_MAP_ADD Set_info( string value )
		{
			request.AddString("info", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_MAP_ADD : GSTypedRequest<LogChallengeEventRequest_MAP_ADD, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_MAP_ADD() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "MAP_ADD");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_MAP_ADD SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_MAP_ADD Set_id( string value )
		{
			request.AddString("id", value);
			return this;
		}
		public LogChallengeEventRequest_MAP_ADD Set_info( string value )
		{
			request.AddString("info", value);
			return this;
		}
	}
	
	public class LogEventRequest_MAP_GET : GSTypedRequest<LogEventRequest_MAP_GET, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_MAP_GET() : base("LogEventRequest"){
			request.AddString("eventKey", "MAP_GET");
		}
	}
	
	public class LogChallengeEventRequest_MAP_GET : GSTypedRequest<LogChallengeEventRequest_MAP_GET, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_MAP_GET() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "MAP_GET");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_MAP_GET SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_MAP_PLAY : GSTypedRequest<LogEventRequest_MAP_PLAY, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_MAP_PLAY() : base("LogEventRequest"){
			request.AddString("eventKey", "MAP_PLAY");
		}
		
		public LogEventRequest_MAP_PLAY Set_code( string value )
		{
			request.AddString("code", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_MAP_PLAY : GSTypedRequest<LogChallengeEventRequest_MAP_PLAY, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_MAP_PLAY() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "MAP_PLAY");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_MAP_PLAY SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_MAP_PLAY Set_code( string value )
		{
			request.AddString("code", value);
			return this;
		}
	}
	
}
	

namespace GameSparks.Api.Messages {


}
