package com.example.groupproject;

import android.widget.Toast;

public class validate {
    Boolean allow = false;
    protected boolean userLoginValidate(String username, String usernameJsonData, String password, String passwordJsonData){
        if(username.equals(usernameJsonData) && password.equals(passwordJsonData)){
            allow = true;
        }else {
            allow = false;
        }
        return allow;
    }
}
