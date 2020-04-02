package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

public class teacherManageAttendanceActivity extends AppCompatActivity {

    @Override
    protected void onRestart() {
        super.onRestart();
        finish();
        startActivity(getIntent());
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_teacher_manage_attendance);
    }
}
