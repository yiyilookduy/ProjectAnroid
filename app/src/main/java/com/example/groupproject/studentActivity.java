package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

public class studentActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_page);
    }

    public void clickToCheckAttendance(View view){
        Intent intent = new Intent(this,studentCheckAttendanceActivity.class);
        startActivity(intent);
    }

    public void clickToCreateTicket(View view){
        Intent intent = new Intent(this,studentCreateTicketActivity.class);
        startActivity(intent);
    }

    public void clickToTrackingTicket(View view){
        Intent intent = new Intent(this,studentTrackingTicketActivity.class);
        startActivity(intent);
    }
}
