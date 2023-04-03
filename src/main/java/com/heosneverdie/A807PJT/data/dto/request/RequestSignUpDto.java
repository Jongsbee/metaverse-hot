package com.heosneverdie.A807PJT.data.dto.request;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RequestSignUpDto {
    private String firebaseId;
    private String nickname;
}


