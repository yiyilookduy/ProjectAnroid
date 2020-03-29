package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.IOException;
import java.util.Objects;

import dto.Student;
import dto.Teacher;
import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import utils.JSONManipulator;

public class MainActivity extends AppCompatActivity {
    private String usernameJsonData = "";
    private String passwordJsonData = "";
    private int roleIdJsonData = 0;
    private Boolean activeJsonData = false;
    validate valid = new validate();
    JSONManipulator jsm = new JSONManipulator();
    Student student = null;
    Teacher teacher = null;
    EditText edtUser,edtPass;;
    Button btnLogin;
    Intent sIntent;
    Intent tIntent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        edtUser = findViewById(R.id.edtUsername);
        edtPass = findViewById(R.id.edtPassword);
        btnLogin = findViewById(R.id.btLogin);
        sIntent = new Intent(this, studentActivity.class);
        tIntent = new Intent(this, teacherActivity.class);

        PostLoginEvent();
    }

    private void PostLoginEvent() {

        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String User = edtUser.getText().toString().trim();
                String Pass = edtPass.getText().toString().trim();
                new PostToServer(User,Pass).execute("http://171.245.197.16:8080/Home/Login");
            }
        });
    }

    class PostToServer extends AsyncTask<String,Void,String> {
        OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
        String user,password;

        public PostToServer(String user, String password) {
            this.user = user;
            this.password = password;
        }

        @Override
        protected String doInBackground(String... strings) {
            RequestBody requestBody = new MultipartBody.Builder()
                    .addFormDataPart("username", user)
                    .addFormDataPart("password", password)
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
            Log.d("JsonData",s);
            super.onPostExecute(s);
            readJsonLoginData(s);
            if(student != null &&teacher != null){
                if(student.getRoleId() == 2 && valid.userLoginValidate(user, student.getUsername(), password, student.getPassword())){
                    startActivity(sIntent);
                }else if(teacher.getRoleId() == 1 && valid.userLoginValidate(user, teacher.getUsername(), password, teacher.getPassword())){
                    startActivity(tIntent);
                }
            }else{
                edtUser.getText().clear();
                edtPass.getText().clear();
            }
        }

        private void readJsonLoginData(String userData){
            try {
                JSONObject jsonObject = new JSONObject(userData);
                String userInfo = jsonObject.getString("Data");
                userInfo= "["+userInfo+"]";
                Log.i("user Data",userInfo);
                JSONArray array = new JSONArray(userInfo);
                for(int i =0;i<array.length();i++){
                    JSONObject jsonPart = array.getJSONObject(i);
                    usernameJsonData = jsonPart.getString("username");
                    passwordJsonData = jsonPart.getString("password");
                    roleIdJsonData = Integer.parseInt(jsonPart.getString("roleId"));
                    activeJsonData = jsonPart.getBoolean("active");
                }
                if (roleIdJsonData == 1){
                    teacher = new Teacher(usernameJsonData,passwordJsonData,roleIdJsonData,activeJsonData);
                }else if (roleIdJsonData == 2){
                    student = new Student(usernameJsonData,passwordJsonData,roleIdJsonData,activeJsonData);
                }else {

                }
            }catch (Exception e){
                e.printStackTrace();
            }
        }
    }
}
