package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import java.io.IOException;
import java.util.Objects;

import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class studentCreateTicketActivity extends AppCompatActivity {
    EditText editTextTeacherId;
    EditText editTextMessageContent;
    Button buttonCreateTicket;
    private String studentId = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_create_ticket);
        editTextTeacherId = findViewById(R.id.edtTeacherId);
        editTextMessageContent = findViewById(R.id.edtMessageContent);
        buttonCreateTicket = findViewById(R.id.btnSubmit);
        
        PostTicketToServerEvent();
    }

    private void PostTicketToServerEvent() {
        buttonCreateTicket.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                studentId = getIntent().getStringExtra("username");
                String teacherId = editTextTeacherId.getText().toString().trim();
                String MessageContent = editTextMessageContent.getText().toString().trim();
                new PostCreateTicketToServer(studentId, teacherId,MessageContent).execute("http://171.245.197.16:8080/Ticket/CreateTicket");
            }
        });
    }

    class PostCreateTicketToServer extends AsyncTask<String,Void,String>{
        OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
        String studentId,teacherId,ContentMessage;

        public PostCreateTicketToServer(String studentId, String teacherId, String ContentMessage) {
            this.studentId = studentId;
            this.teacherId = teacherId;
            this.ContentMessage = ContentMessage;
        }

        @Override
        protected String doInBackground(String... strings) {
            RequestBody requestBody = new MultipartBody.Builder()
                    .addFormDataPart("studentId", studentId)
                    .addFormDataPart("password", teacherId)
                    .addFormDataPart("ContentMessage",ContentMessage)
                    .setType(MultipartBody.FORM).build();
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
