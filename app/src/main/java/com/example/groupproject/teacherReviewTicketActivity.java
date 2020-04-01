package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
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

import dto.Ticket;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

public class teacherReviewTicketActivity extends AppCompatActivity {
    ListView listView;
    String username, JsonContentData, JsonStatusData, JsonIdData,JsonStartDate,JsonEndDate;
    List<Map<String,String>> ticketData;
    List<Ticket> allTicketData;
    SimpleAdapter simpleAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_teacher_review_ticket);
        setTitle("Review tickets");
        username = getIntent().getStringExtra("username");
        listView = findViewById(R.id.lvTeacherReviewTicket);
        ticketData = new ArrayList<>();
        allTicketData = new ArrayList<>();
        new teacherReviewTicketActivity.GetTeacherTicketUrl().execute("http://171.245.197.16:8080/Ticket/GetTicketByTeacherId?teacherId="+username);
    }

    class GetTeacherTicketUrl extends AsyncTask<String,String,String> {
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
            simpleAdapter = new SimpleAdapter(teacherReviewTicketActivity.this, ticketData, android.R.layout.simple_list_item_2, new String[] {"Content","ticket"}, new int[] {android.R.id.text1,android.R.id.text2});
            listView.setAdapter(simpleAdapter);
            listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                    Intent intent = new Intent(teacherReviewTicketActivity.this, teacherEditTicketActivity.class);
                    intent.putExtra("position", position);
                    Ticket t = new Ticket();
                    t = allTicketData.get(position);
                    intent.putExtra("id",t.getId());
                    intent.putExtra("content",t.getContent());
                    intent.putExtra("startDate",t.getStartDate());
                    intent.putExtra("endDate",t.getEndDate());
                    intent.putExtra("status",getStatus());
                    startActivity(intent);
                }
            });
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
                JsonIdData = jsonPart.getString("id");
                JsonContentData = jsonPart.getString("content");
                JsonStartDate = jsonPart.getString("startDate");
                JsonEndDate=jsonPart.getString("endDate");
                JsonStatusData = jsonPart.getString("status");
                Map<String,String> ticketInfo = new HashMap<>();
                Ticket allTicketInfo = new Ticket(JsonIdData,JsonContentData,JsonStartDate,JsonEndDate,JsonStatusData);
                ticketInfo.put("Content","Ticket content: "+JsonContentData);
                ticketInfo.put("ticket","Status: "+JsonStatusData);
                ticketData.add(ticketInfo);
                allTicketData.add(allTicketInfo);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
