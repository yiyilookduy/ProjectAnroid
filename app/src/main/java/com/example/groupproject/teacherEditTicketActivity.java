package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;

import java.io.IOException;
import java.util.Objects;

import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class teacherEditTicketActivity extends AppCompatActivity {
    RadioButton radioButton;
    RadioGroup radioGroup;
    Button btnSubmit;
    String id,content,startDate,endDate,status;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_ticket);
        setTitle("Edit Ticket");

        radioGroup = findViewById(R.id.rdGroup);
        btnSubmit = findViewById(R.id.btnTicketSubmit);
        PostTicketToServerEvent();
    }

    private void PostTicketToServerEvent() {
        btnSubmit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                int radioId = radioGroup.getCheckedRadioButtonId();
                radioButton = findViewById(radioId);
                status = radioButton.getText().toString().trim();
                id = getIntent().getStringExtra("id");
                content = getIntent().getStringExtra("content");
                startDate = getIntent().getStringExtra("startDate");
                endDate = getIntent().getStringExtra("endDate");
                new PostEditTicketToServer(id, content,startDate,endDate,status).execute("http://171.245.197.16:8080/Ticket/UpdateTicket");
                Toast.makeText(teacherEditTicketActivity.this,"Success",Toast.LENGTH_SHORT).show();
                finish();
            }
        });
    }

    class PostEditTicketToServer extends AsyncTask<String,Void,String> {
        OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
        String id,content,startDate,endDate,status;

        public PostEditTicketToServer(String id, String content, String startDate, String endDate, String status) {
            this.id = id;
            this.content = content;
            this.startDate = startDate;
            this.endDate = endDate;
            this.status = status;
        }

        @Override
        protected String doInBackground(String... strings) {
            RequestBody requestBody = new MultipartBody.Builder()
                    .addFormDataPart("id", id)
                    .addFormDataPart("content", content)
                    .addFormDataPart("startDate",startDate)
                    .addFormDataPart("endDate",endDate)
                    .addFormDataPart("status",status)
                    .setType(MultipartBody.FORM)
                    .build();
            Request request = new Request.Builder()
                    .url(strings[0])
                    .post(requestBody)
                    .build();

            try {
                Response response = okHttpClient.newCall(request).execute();
                return Objects.requireNonNull(response.body()).string();
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        protected void onPostExecute(String s) {
            super.onPostExecute(s);
        }
    }
}
