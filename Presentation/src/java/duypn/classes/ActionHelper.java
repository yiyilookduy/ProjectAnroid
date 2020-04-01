/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package duypn.classes;

import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author duypnse63523
 */
public class ActionHelper {
    
    private static final Map<String, String> MapAction;
    
    static{
        MapAction = new HashMap<>();
        MapAction.put("Login", ApplicationContants.LOGINCONTROLLER);
        MapAction.put("Profile", ApplicationContants.PROFILECONTROLLER);
        MapAction.put("UpdateProfile", ApplicationContants.UPDATEPROFILECONTROLLER);
        MapAction.put("Edit", ApplicationContants.EDITCONTROLLER);
        MapAction.put("Search", ApplicationContants.SEARCH_CONTROLLER);
        
        MapAction.put("Attendance", ApplicationContants.ATTENDANCE_CONTROLLER);
    }
    
    public static String Match (String action){
        if(MapAction.containsKey(action)){
            return MapAction.get(action);
        }
        else return ApplicationContants.ERROR;
    }
}
