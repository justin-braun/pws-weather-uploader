using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PWSWeatherUploader
{
    public class WebSocketWeatherFlow
    {
        private static WebSocket ws;
        private string webSocketRootUrl = "wss://ws.weatherflow.com/swd/data?token={0}";
        private string json_startListenAll = "{\"type\":\"listen_start\", \"device_id\":{0}, \"id\":\"2098388936\"}";
        private string json_stopListenAll = "{\"type\":\"listen_stop\", \"device_id\":{0}, \"id\":\"2098388936\"}";
        private string json_startListenRapidWind = "{\"type\":\"listen_rapid_start\", \"device_id\":{0}, \"id\":\"2098388936\"}";
        private string json_stopListenRapidWind = "{\"type\":\"listen_rapid_stop\", \"device_id\":{0}, \"id\":\"2098388936\"}";

        private string Token { get; set; }
        public bool IsListeningAllEnabled { get; private set; } = false;
        public bool IsListeningWindEnabled { get; private set; } = false;
        public WebSocketWeatherFlow(string weatherFlowToken)
        {
            Token = weatherFlowToken;
            StartConnection(Token);
        }

        private void StartConnection(string weatherFlowToken)
        {
            ws = new WebSocket(String.Format(webSocketRootUrl, weatherFlowToken));

            // Setup events
            ws.OnMessage += WebSocket_OnMessage;
            ws.OnError += WebSocket_OnError;
            ws.OnClose += WebSocket_OnClose;

            // Connect
            ws.Connect();

        }

        public void CloseConnection()
        {
            if(!ws.IsAlive)
            {
                ws.Close();
            }
        }
        public void ResetConnection()
        {
            // Close websocket connection
            ws.Close();

            // Release events
            ws.OnMessage -= WebSocket_OnMessage;
            ws.OnError -= WebSocket_OnError;
            ws.OnClose -= WebSocket_OnClose;
            ws = null;

            // Restart connection
            StartConnection(Token);

        }

        public void StartListeningForAllEvents(string deviceId)
        {
            string json_listen_start =  json_startListenAll.Replace("{0}", deviceId);

            if (!ws.IsAlive)
            {
                ResetConnection();
            }

            ws.Send(json_listen_start);
            IsListeningAllEnabled = true;
        }

        public void StopListeningForAllEvents(string deviceId)
        {
            string json_listen_stop = json_stopListenAll.Replace("{0}", deviceId);

            if (ws.IsAlive)
            {
                ws.Send(json_listen_stop);
                IsListeningAllEnabled = false;
            }

        }

        public void StartListeningForRapidWindEvents(string deviceId)
        {
            string json_listen_start = json_startListenRapidWind.Replace("{0}", deviceId);

            if (!ws.IsAlive)
            {
                ResetConnection();
            }

            ws.Send(json_listen_start);
            IsListeningWindEnabled = true;
        }

        public void StopListeningForRapidWindEvents(string deviceId)
        {
            string json_listen_stop = json_stopListenRapidWind.Replace("{0}", deviceId);

            if (ws.IsAlive)
            {
                ws.Send(json_listen_stop);
                IsListeningWindEnabled = false;
            }

        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            if (e.Code == (ushort)CloseStatusCode.Abnormal)
            {
                Console.WriteLine("Connection closed abnormally.");

                // TODO: Try to reconnect
            }
        }


        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            // Deserialize response
            WeatherFlowResponse.Root response = JsonConvert.DeserializeObject<WeatherFlowResponse.Root>(e.Data);

            if(response.Type == "rapid_wind")
            {
                WeatherFlowRapidWindResponse.Root wind_response = JsonConvert.DeserializeObject<WeatherFlowRapidWindResponse.Root>(e.Data);
                TempestRapidWindObservation wind = new TempestRapidWindObservation(string.Join(",", wind_response.Ob));
                DataLogger.SaveWindReading(string.Join(",", wind_response.Ob));

                Console.WriteLine("Rapid Wind Reading:");
                Console.WriteLine(e.Data);
                Console.WriteLine($"wind Speed: {wind.WindSpeed.MsToMph().ToString("0.#")} mph");
                Console.WriteLine();
            }


            else if(response.Type == "obs_st")
            {
                TempestObservation obs = new TempestObservation(string.Join(",", response.Obs[0]));
                Console.WriteLine(e.Data);
                Console.WriteLine($"Obs: {obs.WindGust.MsToMph().ToString("0.#")} mph");
                Console.WriteLine();
            }


            
        }



    }
}
