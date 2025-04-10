// Code generated by protoc-gen-go. DO NOT EDIT.
// versions:
// 	protoc-gen-go v1.36.6
// 	protoc        v6.30.2
// source: payment.proto

package payment_grpc

import (
	protoreflect "google.golang.org/protobuf/reflect/protoreflect"
	protoimpl "google.golang.org/protobuf/runtime/protoimpl"
	reflect "reflect"
	sync "sync"
	unsafe "unsafe"
)

const (
	// Verify that this generated code is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(20 - protoimpl.MinVersion)
	// Verify that runtime/protoimpl is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(protoimpl.MaxVersion - 20)
)

// Запрос на создание заказа
type CreatePaymentRequest struct {
	state         protoimpl.MessageState `protogen:"open.v1"`
	OrderId       int64                  `protobuf:"varint,1,opt,name=order_id,json=orderId,proto3" json:"order_id,omitempty"`                // Идентификатор заказа
	PaybleAmount  int32                  `protobuf:"varint,2,opt,name=payble_amount,json=paybleAmount,proto3" json:"payble_amount,omitempty"` // Стоимость к оплате
	unknownFields protoimpl.UnknownFields
	sizeCache     protoimpl.SizeCache
}

func (x *CreatePaymentRequest) Reset() {
	*x = CreatePaymentRequest{}
	mi := &file_payment_proto_msgTypes[0]
	ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
	ms.StoreMessageInfo(mi)
}

func (x *CreatePaymentRequest) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*CreatePaymentRequest) ProtoMessage() {}

func (x *CreatePaymentRequest) ProtoReflect() protoreflect.Message {
	mi := &file_payment_proto_msgTypes[0]
	if x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use CreatePaymentRequest.ProtoReflect.Descriptor instead.
func (*CreatePaymentRequest) Descriptor() ([]byte, []int) {
	return file_payment_proto_rawDescGZIP(), []int{0}
}

func (x *CreatePaymentRequest) GetOrderId() int64 {
	if x != nil {
		return x.OrderId
	}
	return 0
}

func (x *CreatePaymentRequest) GetPaybleAmount() int32 {
	if x != nil {
		return x.PaybleAmount
	}
	return 0
}

// Запрос на получение платежа
type GetPaymentRequest struct {
	state         protoimpl.MessageState `protogen:"open.v1"`
	OrderId       int64                  `protobuf:"varint,1,opt,name=order_id,json=orderId,proto3" json:"order_id,omitempty"` // Идентификатор заказа
	unknownFields protoimpl.UnknownFields
	sizeCache     protoimpl.SizeCache
}

func (x *GetPaymentRequest) Reset() {
	*x = GetPaymentRequest{}
	mi := &file_payment_proto_msgTypes[1]
	ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
	ms.StoreMessageInfo(mi)
}

func (x *GetPaymentRequest) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*GetPaymentRequest) ProtoMessage() {}

func (x *GetPaymentRequest) ProtoReflect() protoreflect.Message {
	mi := &file_payment_proto_msgTypes[1]
	if x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use GetPaymentRequest.ProtoReflect.Descriptor instead.
func (*GetPaymentRequest) Descriptor() ([]byte, []int) {
	return file_payment_proto_rawDescGZIP(), []int{1}
}

func (x *GetPaymentRequest) GetOrderId() int64 {
	if x != nil {
		return x.OrderId
	}
	return 0
}

// Ответ с идентификатором платежа
type PaymentIdResponse struct {
	state         protoimpl.MessageState `protogen:"open.v1"`
	Id            string                 `protobuf:"bytes,1,opt,name=id,proto3" json:"id,omitempty"` // Идентификатор платежа
	unknownFields protoimpl.UnknownFields
	sizeCache     protoimpl.SizeCache
}

func (x *PaymentIdResponse) Reset() {
	*x = PaymentIdResponse{}
	mi := &file_payment_proto_msgTypes[2]
	ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
	ms.StoreMessageInfo(mi)
}

func (x *PaymentIdResponse) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*PaymentIdResponse) ProtoMessage() {}

func (x *PaymentIdResponse) ProtoReflect() protoreflect.Message {
	mi := &file_payment_proto_msgTypes[2]
	if x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use PaymentIdResponse.ProtoReflect.Descriptor instead.
func (*PaymentIdResponse) Descriptor() ([]byte, []int) {
	return file_payment_proto_rawDescGZIP(), []int{2}
}

func (x *PaymentIdResponse) GetId() string {
	if x != nil {
		return x.Id
	}
	return ""
}

var File_payment_proto protoreflect.FileDescriptor

const file_payment_proto_rawDesc = "" +
	"\n" +
	"\rpayment.proto\x12\apayment\"V\n" +
	"\x14CreatePaymentRequest\x12\x19\n" +
	"\border_id\x18\x01 \x01(\x03R\aorderId\x12#\n" +
	"\rpayble_amount\x18\x02 \x01(\x05R\fpaybleAmount\".\n" +
	"\x11GetPaymentRequest\x12\x19\n" +
	"\border_id\x18\x01 \x01(\x03R\aorderId\"#\n" +
	"\x11PaymentIdResponse\x12\x0e\n" +
	"\x02id\x18\x01 \x01(\tR\x02id2\xa2\x01\n" +
	"\x0ePaymentService\x12J\n" +
	"\rCreatePayment\x12\x1d.payment.CreatePaymentRequest\x1a\x1a.payment.PaymentIdResponse\x12D\n" +
	"\n" +
	"GetPayment\x12\x1a.payment.GetPaymentRequest\x1a\x1a.payment.PaymentIdResponseB)Z\fpayment.grpc\xaa\x02\x18Marketplace.Payment.Grpcb\x06proto3"

var (
	file_payment_proto_rawDescOnce sync.Once
	file_payment_proto_rawDescData []byte
)

func file_payment_proto_rawDescGZIP() []byte {
	file_payment_proto_rawDescOnce.Do(func() {
		file_payment_proto_rawDescData = protoimpl.X.CompressGZIP(unsafe.Slice(unsafe.StringData(file_payment_proto_rawDesc), len(file_payment_proto_rawDesc)))
	})
	return file_payment_proto_rawDescData
}

var file_payment_proto_msgTypes = make([]protoimpl.MessageInfo, 3)
var file_payment_proto_goTypes = []any{
	(*CreatePaymentRequest)(nil), // 0: payment.CreatePaymentRequest
	(*GetPaymentRequest)(nil),    // 1: payment.GetPaymentRequest
	(*PaymentIdResponse)(nil),    // 2: payment.PaymentIdResponse
}
var file_payment_proto_depIdxs = []int32{
	0, // 0: payment.PaymentService.CreatePayment:input_type -> payment.CreatePaymentRequest
	1, // 1: payment.PaymentService.GetPayment:input_type -> payment.GetPaymentRequest
	2, // 2: payment.PaymentService.CreatePayment:output_type -> payment.PaymentIdResponse
	2, // 3: payment.PaymentService.GetPayment:output_type -> payment.PaymentIdResponse
	2, // [2:4] is the sub-list for method output_type
	0, // [0:2] is the sub-list for method input_type
	0, // [0:0] is the sub-list for extension type_name
	0, // [0:0] is the sub-list for extension extendee
	0, // [0:0] is the sub-list for field type_name
}

func init() { file_payment_proto_init() }
func file_payment_proto_init() {
	if File_payment_proto != nil {
		return
	}
	type x struct{}
	out := protoimpl.TypeBuilder{
		File: protoimpl.DescBuilder{
			GoPackagePath: reflect.TypeOf(x{}).PkgPath(),
			RawDescriptor: unsafe.Slice(unsafe.StringData(file_payment_proto_rawDesc), len(file_payment_proto_rawDesc)),
			NumEnums:      0,
			NumMessages:   3,
			NumExtensions: 0,
			NumServices:   1,
		},
		GoTypes:           file_payment_proto_goTypes,
		DependencyIndexes: file_payment_proto_depIdxs,
		MessageInfos:      file_payment_proto_msgTypes,
	}.Build()
	File_payment_proto = out.File
	file_payment_proto_goTypes = nil
	file_payment_proto_depIdxs = nil
}
