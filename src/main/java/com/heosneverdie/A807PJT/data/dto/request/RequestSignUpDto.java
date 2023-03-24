package com.heosneverdie.A807PJT.data.dto.request;

public class RequestSignUpDto {
    private Long id;
    private String nickname;

    public static RequestSignUpDtoBuilder builder() {
        return new RequestSignUpDtoBuilder();
    }

    public Long getId() {
        return this.id;
    }

    public String getNickname() {
        return this.nickname;
    }

    public RequestSignUpDto(final Long id, final String nickname) {
        this.id = id;
        this.nickname = nickname;
    }

    public RequestSignUpDto() {
    }

    public static class RequestSignUpDtoBuilder {
        private Long id;
        private String nickname;

        RequestSignUpDtoBuilder() {
        }

        public RequestSignUpDtoBuilder id(final Long id) {
            this.id = id;
            return this;
        }

        public RequestSignUpDtoBuilder nickname(final String nickname) {
            this.nickname = nickname;
            return this;
        }

        public RequestSignUpDto build() {
            return new RequestSignUpDto(this.id, this.nickname);
        }

        public String toString() {
            return "RequestSignUpDto.RequestSignUpDtoBuilder(id=" + this.id + ", nickname=" + this.nickname + ")";
        }
    }
}


