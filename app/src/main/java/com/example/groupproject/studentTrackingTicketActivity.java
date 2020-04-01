package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.ListView;
import android.widget.SimpleAdapter;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

public class studentTrackingTicketActivity extends AppCompatActivity {
    ListView listView;
    String username, JsonContentData, JsonStatusData;
    List<Map<String,String>> ticketData;
    SimpleAdapter simpleAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_tracking_ticket);
        setTitle("Tracking tickets");
        username = getIntent().getStringExtra("username");
        listView = findViewById(R.id.listViewTracking);
        ticketData = new ArrayList<>();
        new GetUrl().execute("http://171.245.197.16:8080/Ticket/GetTicketByStudentId?studentId="+username);

    }

    class GetUrl extends AsyncTask<String,String,String>{
        OkHttpClient okHttpClient = new OkHttpClient.Builder()
                .connectTimeout(15, TimeUnit.SECONDS)
                .writeTimeout(15, TimeUnit.SECONDS)
                .readTimeout(15, TimeUnit.SECONDS)
                .retryOnConnectionFailure(true)
                .build();

        @Override
        protected String doInBackground(String... strings) {
            Request.Builder builder = new Request.Builder();
            builder.url(strings[0]);
            Request request = builder.build();

            try {
                Response response = okHttpClient.newCall(request).execute();
                return response.body().string();
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        protected void onPostExecute(String s) {
            Log.d("Json data",s);
            readJsonTicketData(s);
            simpleAdapter = new SimpleAdapter(studentTrackingTicketActivity.this, ticketData, android.R.layout.simple_list_item_2, new String[] {"Content","ticket"}, new int[] {android.R.id.text1,android.R.id.text2});
            listView.setAdapter(simpleAdapter);
            super.onPostExecute(s);

        }
    }

    private void readJsonTicketData(String s){
        try {
            JSONObject jsonObject = new JSONObject(s);
            String tickets = jsonObject.getString("Data");
            JSONArray array = new JSONArray(tickets);
            for(int i =0;i<array.length();i++){
                JSONObject jsonPart = array.getJSONObject(i);
                JsonContentData = jsonPart.getString("content");
                JsonStatusData = jsonPart.getString("status");
                Map<String,String> ticketInfo = new HashMap<>();
                ticketInfo.put("Content","Ticket content: "+JsonContentData);
                ticketInfo.put("ticket","Status: "+JsonStatusData);
                ticketData.add(ticketInfo);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
